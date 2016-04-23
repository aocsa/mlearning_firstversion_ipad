#include "pch.h"

namespace IControls
{
	namespace DataSource
	{

		public ref class ChapterDataSource  sealed : public BindableBase
		{
		public:
			ChapterDataSource();

#pragma region Properties

		public:

			property Platform::String^ Title
			{
				void set(Platform::String^ value){ this->_title = value; }
				Platform::String^ get(){ return this->_title; }
			}

			property Platform::String^ Author
			{
				void set(Platform::String^ value){ this->_author = value; }
				Platform::String^ get(){ return this->_author; }
			}

			property Platform::String^ Description
			{
				void set(Platform::String^ value){ this->_description = value; }
				Platform::String^ get(){ return this->_description; }
			}

			property Windows::Foundation::Collections::IObservableVector<SectionDataSource^>^ Sections
			{
				void set(Windows::Foundation::Collections::IObservableVector<SectionDataSource^>^ value){ this->_sections = value; }
				Windows::Foundation::Collections::IObservableVector<SectionDataSource^>^ get(){ return _sections; }
			}

			property Windows::UI::Color ChapterColor
			{
				void set(Windows::UI::Color value){ _chaptercolor = value; }
				Windows::UI::Color get(){ return _chaptercolor; }
			}

			property Windows::UI::Color TemporalColor
			{
				void set(Windows::UI::Color value)
				{
					_temporalcolor = value;
					for (size_t i = 0; i < _sections->Size; i++)
						_sections->GetAt(i)->TemporalColor = value;
				}
				Windows::UI::Color get(){ return _temporalcolor; }
			}

			property Windows::UI::Xaml::Media::Imaging::BitmapImage ^ BackgroundImage
			{
				void set(Windows::UI::Xaml::Media::Imaging::BitmapImage ^value)
				{
					_backgroundimage = value;
					PropertyChanged(this,
						ref new Windows::UI::Xaml::Data::PropertyChangedEventArgs("BackgroundImage"));
				}
				Windows::UI::Xaml::Media::Imaging::BitmapImage^ get(){ return _backgroundimage; }
			}


		private:
			Windows::UI::Xaml::Media::Imaging::BitmapImage ^ _backgroundimage;
			Platform::String^ _title, ^_author, ^_description;
			Windows::UI::Color _chaptercolor, _temporalcolor;
			Windows::Foundation::Collections::IObservableVector<SectionDataSource^>^ _sections;

#pragma endregion
		};

	}
}