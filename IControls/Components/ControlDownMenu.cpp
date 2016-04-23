#include "pch.h"

using namespace IControls::Components;

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;

ControlDownMenu::ControlDownMenu()
{
	init();
}

void ControlDownMenu::init()
{
	_currentindex = 0;

	this->Height = 102.0;
	this->Width = 1600.0;
	this->VerticalAlignment = Windows::UI::Xaml::VerticalAlignment::Bottom;

	_mainscroll = ref new ScrollViewer();
	_mainscroll->HorizontalScrollMode = ScrollMode::Enabled;
	_mainscroll->VerticalScrollMode = ScrollMode::Disabled;
	_mainscroll->VerticalScrollBarVisibility = ScrollBarVisibility::Disabled;
	_mainscroll->HorizontalScrollBarVisibility = ScrollBarVisibility::Auto;
	_mainscroll->Height = 102.0;
	_mainscroll->Width = 1600.0;
	this->Children->Append(_mainscroll);

	_mainpanel = ref new StackPanel();
	_mainpanel->Orientation = Orientation::Horizontal;
	_mainpanel->HorizontalAlignment = Windows::UI::Xaml::HorizontalAlignment::Left;
	_mainscroll->Content = _mainpanel; 

	_iscomponentinit = false;

}

void ControlDownMenu::initstack()
{
	for (size_t i = 0; i < _source->Chapters->Size; i++)
	{
		ControlDownElement ^ elem = ref new ControlDownElement(); 
		elem->Index = i;
		elem->Source = _source->Chapters->GetAt(i);
		elem->ControlDownElementSelected += ref new ControlDownElementSelectedEventHandler(this, &IControls::Components::ControlDownMenu::ControlDown_ElementSelected);
		_mainpanel->Children->Append(elem);
		_elements.push_back(elem);
	}
	_iscomponentinit = true;
	 
}

 
 
void ControlDownMenu::SelectElement(int32 index)
{ 
	if (index != _currentindex)
	{
		_elements[_currentindex]->Unselect();
		_currentindex = index;
		_elements[_currentindex]->Select();
	}
		
}
 

void IControls::Components::ControlDownMenu::ControlDown_ElementSelected(Platform::Object^ sender, int32 index)
{
	SelectElement(index);
	ControlDownElementSelected(this,index);
}