#include "pch.h"

namespace IControls
{
	namespace Components
	{
		//Para el menu parte baja
		public delegate void ControlDownElementSelectedEventHandler(Platform::Object^ sender, int32 index);

		public ref class ControlDownElement sealed : public Windows::UI::Xaml::Controls::Grid
		{
		public:
			ControlDownElement();
			 
			event ControlDownElementSelectedEventHandler ^ ControlDownElementSelected;

			property int32 Index
			{
				void set(int32 value){ this->_index = value; }
				int32 get(){ return _index; }
			}
			 
			property DataSource::ChapterDataSource^ Source
			{
				void set(DataSource::ChapterDataSource ^ value){ _source = value; updatevalues(); }
				DataSource::ChapterDataSource ^ get(){ return _source; }
			}

			property float64 ElementHeight
			{
				void set(float64 value){}
				float64 get(){ return 0.0; }
			}

			property float64 ElementWidth
			{
				void set(float64 value){}
				float64 get(){ return 0.0; }
			}

			void Select();
			void Unselect();

		private:
			int32 _index; 
			float64 _height, _width;
			bool _isselected;
			void Item_Tapped(Platform::Object^ sender, Windows::UI::Xaml::Input::TappedRoutedEventArgs^ e);
			DataSource::ChapterDataSource ^ _source;
			Windows::UI::Xaml::Controls::TextBlock ^ _textname;
			Windows::UI::Xaml::Controls::Grid ^ _container;
			Windows::UI::Xaml::Media::CompositeTransform ^ _transform;
			void init();
			void animate2color(Windows::UI::Color c);
			void animate2double(float64 to, Platform::String ^ prop);
			void updatevalues();
			void OnTapped_1(Platform::Object ^sender, Windows::UI::Xaml::Input::TappedRoutedEventArgs ^e);
		};

	}
}