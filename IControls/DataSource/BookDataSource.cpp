#include "pch.h"

using namespace  IControls::DataSource;

BookDataSource::BookDataSource()
{
	_chapters = ref new Platform::Collections::Vector<ChapterDataSource^>();
}