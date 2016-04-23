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
using namespace Windows::UI::Xaml::Media::Animation;


ControlDownElement::ControlDownElement()
{
	init();
}

void ControlDownElement::init()
{
	
	this->Height = 102.0;
	this->Width = 232.0;

	_container = ref new Grid();
	_container->Width = 228.0;
	_container->Height = 98.0;
	_container->Background = ref new SolidColorBrush(Windows::UI::ColorHelper::FromArgb(100,0,0,0));
	this->Children->Append(_container);
	_transform = ref new CompositeTransform();
	_transform->CenterX = 114.0;
	_transform->CenterY = 49.0;
	_container->RenderTransform = _transform;

	_textname = ref new TextBlock();
	_textname->Height = 30;
	_textname->Foreground = ref new SolidColorBrush(Windows::UI::Colors::White);
	_textname->FontSize = 18;
	_textname->TextAlignment = TextAlignment::Center;
	_textname->Text = "Capitulo n";
	_container->Children->Append(_textname);
	 
	this->Tapped += ref new Windows::UI::Xaml::Input::TappedEventHandler(this, &IControls::Components::ControlDownElement::OnTapped_1);
}

void ControlDownElement::Select()
{
	_isselected = true;
	animate2color(_source->ChapterColor);
	animate2double(1.04, "ScaleX");
	animate2double(1.04, "ScaleY");
}

void ControlDownElement::Unselect()
{ 
	_isselected = false;
	animate2color(Windows::UI::ColorHelper::FromArgb(100 , 0,0,0));
	animate2double(1.0, "ScaleX");
	animate2double(1.0, "ScaleY");
}
void ControlDownElement::updatevalues()
{
	_textname->Text = _source->Title; 
}


void ControlDownElement::animate2color(Windows::UI::Color c)
{
	Storyboard ^ story = ref new Storyboard();
	ColorAnimation^ animation = ref new ColorAnimation();
	Windows::Foundation::TimeSpan ts;
	ts.Duration = 3500000;
	Windows::UI::Xaml::Duration dur(ts);
	animation->Duration = dur;
	story->Children->Append(animation);
	animation->EnableDependentAnimation = true;

	Storyboard::SetTarget(animation, _container);
	Storyboard::SetTargetProperty(animation, "(Grid.Background).(SolidColorBrush.Color)");

	animation->To = c;
	story->Begin(); 
}


void ControlDownElement::animate2double(float64 to, Platform::String ^ prop)
{
	Storyboard ^ story = ref new Storyboard();
	DoubleAnimation^ animation = ref new DoubleAnimation();
	Windows::Foundation::TimeSpan ts;
	ts.Duration = 3500000;
	Windows::UI::Xaml::Duration dur(ts);
	animation->Duration = dur;
	story->Children->Append(animation);
	animation->EnableDependentAnimation = true;

	Storyboard::SetTarget(animation, _transform);
	Storyboard::SetTargetProperty(animation, prop);

	animation->To = to;
	story->Begin();
}
 


void IControls::Components::ControlDownElement::OnTapped_1(Platform::Object ^sender, Windows::UI::Xaml::Input::TappedRoutedEventArgs ^e)
{ 
	ControlDownElementSelected(this, this->_index); 
}