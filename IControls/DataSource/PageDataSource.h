#include "pch.h"

namespace IControls
{
	namespace DataSource
	{

		public ref class PageDataSource  sealed : public BindableBase
		{
		public:
			PageDataSource();

#pragma region Properties

		public:

			property Platform::String^ Name
			{
				void set(Platform::String^ value){ this->_name = value; }
				Platform::String^ get(){
					return this->_name; PropertyChanged(this,
						ref new Windows::UI::Xaml::Data::PropertyChangedEventArgs("Name"));
				}
			}


			property Platform::String^ Description
			{
				void set(Platform::String^ value){ this->_description = value; }
				Platform::String^ get(){ return this->_description; }
			}


			property Windows::UI::Xaml::Media::Imaging::BitmapImage ^ ImageContent
			{
				void set(Windows::UI::Xaml::Media::Imaging::BitmapImage ^value)
				{
					_imagecontent = value;
					PropertyChanged(this,
						ref new Windows::UI::Xaml::Data::PropertyChangedEventArgs("ImageSource"));
				}
				Windows::UI::Xaml::Media::Imaging::BitmapImage^ get(){ return _imagecontent; }
			}


			property Windows::UI::Color BorderColor
			{
				void set(Windows::UI::Color value){
					_bordercolor = value;
					PropertyChanged(this,
						ref new Windows::UI::Xaml::Data::PropertyChangedEventArgs("BorderColor"));
				}
				Windows::UI::Color get(){ return _bordercolor; }
			}

		private:


			Platform::String^ _name, ^ _description;
			Windows::UI::Xaml::Media::Imaging::BitmapImage^ _imagecontent;
			Windows::UI::Color _bordercolor;

#pragma endregion
		};

	}
}