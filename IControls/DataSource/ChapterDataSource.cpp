#include "pch.h"

using namespace  IControls::DataSource;

ChapterDataSource::ChapterDataSource()
{ 
	_sections = ref new Platform::Collections::Vector<SectionDataSource^>();
}