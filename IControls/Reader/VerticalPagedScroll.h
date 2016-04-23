#include "pch.h"

namespace IControls
{
	namespace Reader
	{
		public ref class VerticalPagedScroll sealed : public Windows::UI::Xaml::Controls::Grid,
			Windows::UI::Xaml::Data::INotifyPropertyChanged
		{
		public:
			VerticalPagedScroll();
			virtual event Windows::UI::Xaml::Data::PropertyChangedEventHandler^ PropertyChanged;

#pragma region Private Properties

		private:
			virtual void OnPropertyChanged(Platform::String^ propertyName);

#pragma endregion

		};
	}
}