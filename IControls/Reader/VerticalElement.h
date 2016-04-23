#include "pch.h"

namespace IControls
{
	namespace Reader
	{
		public ref class VerticalElement sealed : public Windows::UI::Xaml::Controls::Grid,
			Windows::UI::Xaml::Data::INotifyPropertyChanged
		{
		public:
			VerticalElement();
			virtual event Windows::UI::Xaml::Data::PropertyChangedEventHandler^ PropertyChanged;

#pragma region Private Properties

		private:
			virtual void OnPropertyChanged(Platform::String^ propertyName);

#pragma endregion

		};
	}
}