#include "pch.h"

namespace IControls
{
	namespace Components
	{
		public ref class ControlDownMenu sealed : public Windows::UI::Xaml::Controls::Grid
		{
		public:
			ControlDownMenu(); 
			event ControlDownElementSelectedEventHandler ^ ControlDownElementSelected;

			property DataSource::BookDataSource^ Source
			{
				void set(DataSource::BookDataSource ^ value)
				{
					_source = value;
					initstack();
					
				}
				DataSource::BookDataSource ^ get(){ return _source; }
			}


			void SelectElement(int32 index);

		private:
			 
			DataSource::BookDataSource^ _source;
			int32 _currentindex;
			Windows::UI::Xaml::Controls::ScrollViewer ^ _mainscroll;
			Windows::UI::Xaml::Controls::StackPanel ^ _mainpanel;
			std::vector<ControlDownElement^> _elements;

			void init();
			bool _iscomponentinit;
			void initstack();  

			void ControlDown_ElementSelected(Platform::Object^ sender, int32 index);

		};

	}
}