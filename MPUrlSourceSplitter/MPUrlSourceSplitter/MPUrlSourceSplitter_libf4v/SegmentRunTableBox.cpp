/*
    Copyright (C) 2007-2010 Team MediaPortal
    http://www.team-mediaportal.com

    This file is part of MediaPortal 2

    MediaPortal 2 is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal 2 is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal 2.  If not, see <http://www.gnu.org/licenses/>.
*/

#include "StdAfx.h"

#include "SegmentRunTableBox.h"

CSegmentRunTableBox::CSegmentRunTableBox(void)
  :CBox()
{
  this->version = 0;
  this->flags = 0;
  this->segmentRunEntryTable = new CSegmentRunEntryCollection();
  this->qualitySegmentUrlModifiers = new CQualitySegmentUrlModifierCollection();
}

CSegmentRunTableBox::~CSegmentRunTableBox(void)
{
  FREE_MEM_CLASS(this->segmentRunEntryTable);
  FREE_MEM_CLASS(this->qualitySegmentUrlModifiers);
}

bool CSegmentRunTableBox::Parse(const unsigned char *buffer, unsigned int length)
{
  this->version = 0;
  this->flags = 0;
  if (this->segmentRunEntryTable != NULL)
  {
    this->segmentRunEntryTable->Clear();
  }
  if (this->qualitySegmentUrlModifiers != NULL)
  {
    this->qualitySegmentUrlModifiers->Clear();
  }

  bool result = (this->segmentRunEntryTable != NULL) && (this->qualitySegmentUrlModifiers != NULL);
  // in bad case we don't have tables, but still it can be valid box
  result &= __super::Parse(buffer, length);

  if (result)
  {
    if (wcscmp(this->type, SEGMENT_RUN_TABLE_BOX_TYPE) != 0)
    {
      // incorect box type
      this->parsed = false;
      result = false;
    }
    else
    {
      // box is bootstrap info box, parse all values
      unsigned int position = this->HasExtendedHeader() ? BOX_HEADER_LENGTH_SIZE64 : BOX_HEADER_LENGTH;

      // until version and flags end is 4 bytes
      bool continueParsing = ((position + 4) <= length);
      
      if (continueParsing)
      {
        this->version = BE8(buffer + position);
        position++;

        this->flags = BE24(buffer + position);
        position += 3;
      }

      continueParsing &= (position < length);
      if (continueParsing)
      {
        // quality entry count and quality segment url modifiers
        unsigned int qualityEntryCount = BE8(buffer + position);
        position++;
        continueParsing &= (position < length);

        for(unsigned int i = 0; continueParsing && (i < qualityEntryCount); i++)
        {
          unsigned int positionAfter = position;
          wchar_t *qualitySegmentUrlModifier = NULL;
          continueParsing &= SUCCEEDED(this->GetString(buffer, length, position, &qualitySegmentUrlModifier, &positionAfter));

          if (continueParsing)
          {
            position = positionAfter;

            // create quality segment url modifier item in quality segment url modifier collection
            CQualitySegmentUrlModifier *qualitySegmentUrlModifierEntry = new CQualitySegmentUrlModifier(qualitySegmentUrlModifier);
            continueParsing &= this->qualitySegmentUrlModifiers->Add(qualitySegmentUrlModifierEntry);
          }

          FREE_MEM(qualitySegmentUrlModifier);

          continueParsing &= (position < length);
        }
      }

      continueParsing &= ((position + 4) < length);
      if (continueParsing)
      {
        // segment run entry count and segment run entry table
        unsigned int segmentRunEntryCount = BE32(buffer + position);
        position += 4;

        for(unsigned int i = 0; continueParsing && (i < segmentRunEntryCount); i++)
        {
          // need to read 8 bytes
          // but this segment can be last in buffer
          continueParsing &= ((position + 7 ) < length);

          if (continueParsing)
          {
            unsigned int firstSegment = BE32(buffer + position);
            position += 4;

            unsigned int fragmentsPerSegment = BE32(buffer + position);
            position += 4;

            CSegmentRunEntry *segment = new CSegmentRunEntry(firstSegment, fragmentsPerSegment);
            continueParsing &= this->segmentRunEntryTable->Add(segment);
          }
        }
      }

      if (!continueParsing)
      {
        // not correctly parsed
        this->parsed = false;
      }
    }
  }

  result = this->parsed;

  return result;
}

wchar_t *CSegmentRunTableBox::GetParsedHumanReadable(wchar_t *indent)
{
  wchar_t *result = NULL;
  wchar_t *previousResult = __super::GetParsedHumanReadable(indent);

  if ((previousResult != NULL) && (this->IsParsed()))
  {
    // prepare quality segment url modifier collection
    wchar_t *qualitySegmentUrlModifier = NULL;
    wchar_t *tempIndent = FormatString(L"%s\t", indent);
    for (unsigned int i = 0; i < this->qualitySegmentUrlModifiers->Count(); i++)
    {
      CQualitySegmentUrlModifier *qualitySegmentUrlModifierEntry = this->qualitySegmentUrlModifiers->GetItem(i);
      wchar_t *tempqualitySegmentUrlModifierEntry = FormatString(
        L"%s%s%s'%s'",
        (i == 0) ? L"" : qualitySegmentUrlModifier,
        (i == 0) ? L"" : L"\n",
        tempIndent,
        qualitySegmentUrlModifierEntry->GetQualitySegmentUrlModifier()
        );
      FREE_MEM(qualitySegmentUrlModifier);

      qualitySegmentUrlModifier = tempqualitySegmentUrlModifierEntry;
    }

    // prepare segment run entry table
    wchar_t *segmentRunEntry = NULL;
    for (unsigned int i = 0; i < this->segmentRunEntryTable->Count(); i++)
    {
      CSegmentRunEntry *segmentRunEntryEntry = this->segmentRunEntryTable->GetItem(i);
      wchar_t *tempSegmentRunEntry = FormatString(
        L"%s%s%sFirst segment: %d, fragments per segment: %d",
        (i == 0) ? L"" : segmentRunEntry,
        (i == 0) ? L"" : L"\n",
        tempIndent,
        segmentRunEntryEntry->GetFirstSegment(),
        segmentRunEntryEntry->GetFragmentsPerSegment()
        );
      FREE_MEM(segmentRunEntry);

      segmentRunEntry = tempSegmentRunEntry;
    }
    FREE_MEM(tempIndent);

    // prepare finally human readable representation
    result = FormatString( \
      L"%s\n" \
      L"%sVersion: %d\n" \
      L"%sFlags: 0x%06X\n" \
      L"%sQuality entry count: %d\n" \
      L"%s%s" \
      L"%sSegment run entry count: %d\n" \
      L"%s",
      
      previousResult,
      indent, this->version,
      indent, this->flags,
      indent, this->qualitySegmentUrlModifiers->Count(),
      (qualitySegmentUrlModifier == NULL) ? L"" : qualitySegmentUrlModifier, (qualitySegmentUrlModifier == NULL) ? L"" : L"\n",
      indent, this->segmentRunEntryTable->Count(),
      (segmentRunEntry == NULL) ? L"" : segmentRunEntry
      
      );

    FREE_MEM(qualitySegmentUrlModifier);
    FREE_MEM(segmentRunEntry);
  }

  FREE_MEM(previousResult);

  return result;
}

CQualitySegmentUrlModifierCollection *CSegmentRunTableBox::GetQualitySegmentUrlModifiers(void)
{
  return this->qualitySegmentUrlModifiers;
}

CSegmentRunEntryCollection *CSegmentRunTableBox::GetSegmentRunEntryTable(void)
{
  return this->segmentRunEntryTable;
}