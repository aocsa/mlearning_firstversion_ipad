#include "pch.h"

namespace IControls
{
	namespace DataSource
	{
		public ref class BookDataSource  sealed : public BindableBase
		{
		public:
			BookDataSource();

#pragma region Properties

		public:

			property Platform::String^ Title
			{
				void set(Platform::String^ value){ this->_title = value; }
				Platform::String^ get(){ return this->_title; }
			}

			property Windows::Foundation::Collections::IObservableVector<ChapterDataSource^>^ Chapters
			{
				void set(Windows::Foundation::Collections::IObservableVector<ChapterDataSource^>^ value){ this->_chapters = value; }
				Windows::Foundation::Collections::IObservableVector<ChapterDataSource^>^ get(){ return _chapters; }
			}
			 
			property Windows::UI::Color TemporalColor
			{
				void set(Windows::UI::Color value)
				{
					_temporalcolor = value;
					for (size_t i = 0; i < _chapters->Size; i++)
						_chapters->GetAt(i)->TemporalColor = value;
				}
				Windows::UI::Color get(){ return _temporalcolor; }
			}

		private:
			Windows::UI::Color _chaptercolor, _temporalcolor;
			Platform::String^ _title;
			Windows::Foundation::Collections::IObservableVector<ChapterDataSource^>^ _chapters;

#pragma endregion
		};

	}
}