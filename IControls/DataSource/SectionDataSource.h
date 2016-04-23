#include "pch.h"
namespace IControls
{

	namespace DataSource
	{

		public ref class SectionDataSource sealed : public BindableBase
		{
		public:
			SectionDataSource();

#pragma region Properties

		public:

			property Platform::String^ Name
			{
				void set(Platform::String^ value){ this->_name = value; }
				Platform::String^ get(){ return this->_name; }
			}

			property Platform::String^ Description
			{
				void set(Platform::String^ value){ this->_description = value; }
				Platform::String^ get(){ return this->_description; }
			}

			property Windows::Foundation::Collections::IObservableVector<PageDataSource^>^ Pages
			{
				void set(Windows::Foundation::Collections::IObservableVector<PageDataSource^>^ value){ this->_pages = value; }
				Windows::Foundation::Collections::IObservableVector<PageDataSource^>^ get(){ return _pages; }
			}

			property Windows::UI::Color TemporalColor
			{
				void set(Windows::UI::Color value)
				{
					_temporalcolor = value;
					for (size_t i = 0; i < _pages->Size; i++)
						_pages->GetAt(i)->BorderColor = _temporalcolor;
				}
				Windows::UI::Color get(){ return _temporalcolor; }
			}

		private:
			Platform::String^ _name;
			Platform::String^ _description;
			Windows::UI::Color  _temporalcolor;
			Windows::Foundation::Collections::IObservableVector<PageDataSource^>^ _pages;

#pragma endregion

		};

	}
}