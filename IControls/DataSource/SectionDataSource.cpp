#include "pch.h"

using namespace IControls::DataSource;

SectionDataSource::SectionDataSource()
{ 
	_pages = ref new Platform::Collections::Vector<PageDataSource^>();
}