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

ChapterHeaderControl::ChapterHeaderControl()
{
	init();
}


void ChapterHeaderControl::init()
{
	this->Height = 376;
	this->Width = 376;

	panelgrid = ref new StackPanel();
	panelgrid->Orientation = Orientation::Vertical;
	Children->Append(panelgrid);

	_title = ref new TextBlock();
	_title->FontSize = 33;
	_title->FontWeight = Windows::UI::Text::FontWeights::Thin;
	_title->TextWrapping = TextWrapping::Wrap;
	_title->Height = 88.0;
	_title->Width = 366.0;
	_title->Foreground = ref new SolidColorBrush(Windows::UI::Colors::Gray);
	_title->Text = "EXPERIENCIA DE USUARIO";
	panelgrid->Children->Append(_title);

	_tags = ref new TextBlock();
	_tags->FontSize = 33;
	_tags->FontWeight = Windows::UI::Text::FontWeights::Black;
	_tags->Height = 52.0;
	_tags->Width = 366.0;
	_tags->Foreground = ref new SolidColorBrush(Windows::UI::Colors::Aquamarine);
	_tags->Text = "INMERSIVA";
	panelgrid->Children->Append(_tags);

	_author = ref new TextBlock();
	_author->FontSize = 33;
	_author->TextWrapping = TextWrapping::Wrap;
	_author->Height = 58.0;
	_author->Width = 366.0;
	_author->Foreground = ref new SolidColorBrush(Windows::UI::Colors::Gray);
	_author->Text = "Sensacional e intuitivo";
	panelgrid->Children->Append(_author);

	_description = ref new TextBlock();
	_description->FontSize = 19;
	_description->FontWeight = Windows::UI::Text::FontWeights::Light;
	_description->TextWrapping = TextWrapping::Wrap;
	_description->Height = 178.0;
	_description->Width = 366.0;
	_description->Foreground = ref new SolidColorBrush(Windows::UI::Colors::Gray);
	_description->Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.\n Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat";
	panelgrid->Children->Append(_description);


	Grid^ linev = ref new Grid();
	linev->Height = 350.0;
	linev->Width = 1.0;
	linev->Background = ref new SolidColorBrush(Windows::UI::Colors::Gray);
	linev->HorizontalAlignment = Windows::UI::Xaml::HorizontalAlignment::Right;
	Children->Append(linev);
}

void ChapterHeaderControl::setColor()
{
	_tags->Foreground = ref new SolidColorBrush(_chaptercolor);
}