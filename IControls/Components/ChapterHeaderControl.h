#include "pch.h"

namespace IControls
{
	namespace Components
	{
		public ref class ChapterHeaderControl sealed : public Windows::UI::Xaml::Controls::Grid
		{
		public:
			ChapterHeaderControl();
			 

			property Platform::String^ Title
			{
				void set(Platform::String^ value){ this->_title->Text = value; }
				Platform::String^ get(){ return this->_title->Text; }
			}

			property Platform::String^ Author
			{
				void set(Platform::String^ value){ this->_author->Text = value; }
				Platform::String^ get(){ return this->_author->Text; }
			}

			property Platform::String^ Description
			{
				void set(Platform::String^ value){ this->_description->Text = value; }
				Platform::String^ get(){ return this->_description->Text; }
			}

			property Windows::UI::Color ChapterColor
			{
				void set(Windows::UI::Color value){ _chaptercolor = value; setColor(); }
				Windows::UI::Color get(){ return _chaptercolor; }
			}



		private : 

			void init();
			void setColor();
			 
			Windows::UI::Color _chaptercolor;

			Windows::UI::Xaml::Controls::TextBlock ^ _title, ^_tags, ^_author, ^_description;
			Windows::UI::Xaml::Controls::StackPanel ^ panelgrid;

		};
	}
}