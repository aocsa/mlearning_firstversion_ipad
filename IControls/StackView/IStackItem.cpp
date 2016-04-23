#include "pch.h"

using namespace IControls::StackView;


using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Media::Imaging ;
using namespace Windows::UI::Xaml::Navigation;
using namespace Windows::UI::Xaml::Media::Animation;

IStackItem::IStackItem()
{
	initcontrols();
	initproperties(); 
	inititemanimations();  
}


void IControls::StackView::IStackItem::OnPropertyChanged(Platform::Object ^sender, Windows::UI::Xaml::Data::PropertyChangedEventArgs ^e)
{ 
	animatecolor();
}


void IStackItem::initcontrols()
{
	this->HorizontalAlignment =  Windows::UI::Xaml::HorizontalAlignment::Left ;
	this->VerticalAlignment = Windows::UI::Xaml::VerticalAlignment::Top ;
	this->Width = _borderwidth ;
	this->Height = _borderheight ;

	this->_transform  =  ref new CompositeTransform();
	this->RenderTransform = this->_transform ;

	/**this->_borderimage = ref new Image();
	this->_borderimage->Stretch =  Stretch::Fill ;
	this->_borderimage->Width = _borderwidth  ;
	this->_borderimage->Height = _borderheight ;
	_borderimage->Opacity = 0.0;
	this->Children->Append(_borderimage);*/

	//init the border Color
	_bordercolor = ref new Border();
	this->_bordercolor->Width = _borderwidth;
	this->_bordercolor->Height = _borderheight;
	_bordercolor->BorderThickness = Thickness(10);
	_bordercolor->CornerRadius = CornerRadius(4);
	//_bordercolor->Background = ref new SolidColorBrush(Windows::UI::Colors::Aqua);
	
	this->Children->Append(_bordercolor);

	this->_itemgrid = ref new Grid();
	this->Children->Append(_itemgrid);
	this->_itemgrid->Height = _thumbheight;
	this->_itemgrid->Width = _thumbwidth;

	this->_thumbimage =  ref new Image();
	this->_thumbimage->Height = _thumbheight ;
	this->_thumbimage->Width = _thumbwidth ;
	this->_thumbimage->Stretch =  Stretch::Fill ;
	this->Children->Append(_thumbimage);
 
	//for touch mainpualtions
	this->PointerPressed += ref new PointerEventHandler(this,&IControls::StackView::IStackItem::StackItem_PointerPressed );
	this->PointerReleased += ref new PointerEventHandler(this,&IControls::StackView::IStackItem::StackItem_PointerReleased );
	this->Tapped += ref new TappedEventHandler(this , &IControls::StackView::IStackItem::StackItem_Tapped);
	 
	//for text and description
	_textpanel = ref new StackPanel();
	_textpanel->VerticalAlignment = Windows::UI::Xaml::VerticalAlignment::Center;
	_textpanel->HorizontalAlignment = Windows::UI::Xaml::HorizontalAlignment::Center;
	_textpanel->Orientation = Orientation::Vertical;
	_textpanel->Height = 120;
	_textpanel->Width = 140; 
	_textpanel->Opacity = 0.0;

	_titletext = ref new TextBlock();
	_titletext->Height = 40;
	_titletext->Width = 140;
	_titletext->Text = "Nombre de Item";
	_titletext->TextWrapping = TextWrapping::Wrap;
	_titletext->TextAlignment = TextAlignment::Center;
	_textpanel->Children->Append(_titletext);
	 
	_descriptiontext = ref new TextBlock();
	_descriptiontext->Height = 80;
	_descriptiontext->Width = 140;
	_descriptiontext->Text = "Descripcion del Item";
	_descriptiontext->TextWrapping = TextWrapping::Wrap;
	_descriptiontext->TextAlignment = TextAlignment::Center;
	_textpanel->Children->Append(_descriptiontext);

	_texttransform = ref new CompositeTransform();
	_textpanel->RenderTransform = _texttransform;
	//
	this->Children->Append(_textpanel);

}

#pragma region Public Methods 

//this function must be called after to set a not null  data source
void IStackItem::LoadThumbSource()
{
	if (_datasource->ImageContent != nullptr)
		this->_thumbimage->Source = _datasource->ImageContent;
	_titletext->Text = _datasource->Name;
	_descriptiontext->Text = _datasource->Description;
	_borderbrushcolor = ref new SolidColorBrush(_datasource->BorderColor);
	_bordercolor->BorderBrush = _borderbrushcolor;

	_datasource->PropertyChanged += ref new Windows::UI::Xaml::Data::PropertyChangedEventHandler(this, &IControls::StackView::IStackItem::datasourcePropertyChanged);
}


void IControls::StackView::IStackItem::datasourcePropertyChanged(Platform::Object ^sender, Windows::UI::Xaml::Data::PropertyChangedEventArgs ^e)
{ 
	if (e->PropertyName == "ImageSource")
	{
		if (_datasource->ImageContent != nullptr)
			this->_thumbimage->Source = _datasource->ImageContent;
	}

	if (e->PropertyName == "BorderColor")
	{
		animatecolor();
	}
}

void IStackItem::LoadBorder()
{
	//not cal this
	//this->_borderimage->Source =  ref new BitmapImage(ref new Uri(this->_bordersource));
}

void IStackItem::LoadFullSource()
{
	/**Image ^ img  =  ref new Image();  
	if (_datasource->MediumSource != nullptr)
		img->Source = ref new BitmapImage(ref new Uri(_datasource->MediumSource));
	img->Stretch = Stretch::Fill ;  
	_itemgrid->Children->Append(img);
	Canvas::SetZIndex(_itemgrid, 10);
	_itemimage = img ; */
}

void IStackItem::DeleteFullSource()
{
	Canvas::SetZIndex(_itemgrid, 0);
	_itemimage = nullptr ; 
	_itemgrid->Children->Clear();
	_touches = 0 ;
	
}

void IStackItem::ItemManipulationCompleted()
{
	if(_transform->ScaleX < _maxscale/2)
		AnimateToThumb();
	else
		AnimateToFull();
}

void IStackItem::AnimateToOpen()
{
	this->_translateXanimation->To = this->_finalposition ;
	this->_rotateanimation->To = 0.0 ;
	this->_rotatestory->Begin();
	this->_translatestory->Begin();  
	this->_isopen = true ;
}

void IStackItem::AnimateToClose()
{
	this->_translateXanimation->To = this->_initialposition ;
	this->_rotateanimation->To = this->_initialangle ;
	this->_rotatestory->Begin();
	this->_translatestory->Begin();  
	this->_isopen =  false ;
}

void IStackItem::SetToOpen()
{
	this->_transform->TranslateX =this->_finalposition ; 
	this->_transform->TranslateY= 0.0 ;
	this->_transform->Rotation = 0.0 ;
	this->_transform->ScaleX = 1.0 ;
	this->_transform->ScaleY = 1.0 ;
	this->_isopen = true ;
	_texttransform->TranslateY = 180.0;//text position
	_textpanel->Opacity = 1.0;
	DeleteFullSource();
}

void IStackItem::SetToClose()
{
	this->_transform->TranslateX =this->_initialposition ; 
	this->_transform->TranslateY= 0.0 ;
	this->_transform->Rotation = this->_initialangle ;
	this->_transform->ScaleX = 1.0 ;
	this->_transform->ScaleY = 1.0 ;
	this->_isopen = false ;
	_texttransform->TranslateY = 0.0;
	_textpanel->Opacity = 0.0;
	DeleteFullSource();
}

void IStackItem::AnimateToFull()
{
	this->ZIndex = _maxthreshold ;
	this->_translatexanimation1->To = this->_fullpositionx ;
	this->_translateyanimation1->To = this->_fullpositiony ;
	this->_rotateanimation1->To = 0.0 ;
	this->_scalexanimation1->To = this->_maxscale + 1.5;
	this->_scaleyanimation1->To = this->_maxscale + 1.5;
	this->_translatestory1->Begin();
	this->_rotatestory1->Begin();
	this->_scalestory1->Begin(); 
	StackItemFullAnimationStarted(this , _chapter , _section ,  _itemnumber);
	_ismfull = true ;
}

void IStackItem::SetToFull()
{
	this->_transform->TranslateX = this->_fullpositionx;
	this->_transform->TranslateY =  this->_fullpositiony ;
	this->_transform->Rotation = 0.0 ;
	this->_transform->ScaleX = this->_maxscale ;
	this->_transform->ScaleY = this->_maxscale ;
	this->_isfull =  true ;
	//LoadFullSource();
	ZIndex = _maxthreshold ;
}

void IStackItem::AnimateToThumb()
{
	this->ZIndex = _maxthreshold ;
	this->_translatexanimation1->To = this->_finalposition ;
	this->_translateyanimation1->To = 0.0 ;
	this->_rotateanimation1->To = 0.0 ;
	this->_scalexanimation1->To = 1.0 ;
	this->_scaleyanimation1->To = 1.0 ;
	this->_translatestory1->Begin();
	this->_rotatestory1->Begin();
	this->_scalestory1->Begin(); 
	StackItemThumbAnimationStarted(this, _chapter , _section, _itemnumber);
}

void IStackItem::SetToThumb()
{
	this->_transform->TranslateX = this->_finalposition ;
	this->_transform->TranslateY =  0.0 ;
	this->_transform->Rotation = 0.0 ;
	this->_transform->ScaleX = 1.0 ;
	this->_transform->ScaleY = 1.0 ;
	this->_isfull = false ;
	ZIndex = _maxthreshold - _itemnumber ;
}

void IStackItem::ShowText()
{
	animate2double(180.0);//text position
	animate2opacity(1.0);
}

void IStackItem::HiddeText()
{
	if (_textpanel->Opacity > 0.5)
	{
		animate2double(0.0);
		//animate2opacity(0.0);
		_textpanel->Opacity = 0.0;
	}
}

#pragma endregion

#pragma region Private Methods

void IStackItem::animatecolor()
{

	Storyboard ^ story = ref new Storyboard();
	ColorAnimation^ animation = ref new ColorAnimation();
	Windows::Foundation::TimeSpan ts;
	ts.Duration = 8500000;
	Windows::UI::Xaml::Duration dur(ts);
	animation->Duration = dur;
	story->Children->Append(animation);
	animation->EnableDependentAnimation = true;

	Storyboard::SetTarget(animation, _bordercolor);
	Storyboard::SetTargetProperty(animation, "(Border.BorderBrush).(SolidColorBrush.Color)");

	animation->To = _datasource->BorderColor;
	story->Begin();
}


void IStackItem::animate2double(float64 to)
{ 
	Storyboard ^ story = ref new Storyboard();
	DoubleAnimation^ animation = ref new DoubleAnimation();
	Windows::Foundation::TimeSpan ts;
	ts.Duration = 2500000;
	Windows::UI::Xaml::Duration dur(ts);
	animation->Duration = dur;
	story->Children->Append(animation);
	animation->EnableDependentAnimation = true;

	Storyboard::SetTarget(animation, _texttransform);
	Storyboard::SetTargetProperty(animation, "TranslateY");

	animation->To = to;
	story->Begin();
}

void IStackItem::animate2opacity(float64 to)
{
	Storyboard ^ story = ref new Storyboard();
	DoubleAnimation^ animation = ref new DoubleAnimation();
	Windows::Foundation::TimeSpan ts;
	ts.Duration = 4500000;
	Windows::UI::Xaml::Duration dur(ts);
	animation->Duration = dur;
	story->Children->Append(animation);
	animation->EnableDependentAnimation = true; 
	Storyboard::SetTarget(animation, _textpanel);
	Storyboard::SetTargetProperty(animation, "Opacity");

	animation->To = to;
	story->Begin();
}


void IStackItem::initproperties()
{
	_itemnumber = 0 ;
	_initialangle  = 0.0 ;
	_datasource  = nullptr;
	_bordersource = nullptr ;

	_thumbheight = 0;
	_thumbwidth = 0;
	_borderheight = 0;
	_borderwidth = 0.0 ;
	_initialposition = 0 ;
	_finalposition = 0 ;

	_isopen = false ; 
	_isfull =  false  ;

	_touches = 0 ;
	_maxthreshold = 100 ;

	_ismfull = false;
}

#pragma endregion

#pragma region Events Methods
 
void IControls::StackView::IStackItem::StackItem_PointerPressed(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e)
{
	this->_touches += 1 ;
	if(_touches > 1 && _isopen)
	{
		StackItemSelected(this , this->_itemnumber);
	}
}

void IControls::StackView::IStackItem::StackItem_PointerReleased(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e)
{
	_touches = 0 ;
}

void IControls::StackView::IStackItem::StackItem_Tapped(Platform::Object^ sender, Windows::UI::Xaml::Input::TappedRoutedEventArgs^ e)
{
	if(_isopen)
	{
		StackItemTapped(this, this->_itemnumber); 		
		ZIndex = _maxthreshold ;
		//LoadFullSource();
	}
	this->_touches = 0  ;
}
void IControls::StackView::IStackItem::StackItem_StoryboardCompleted(Platform::Object^ sender, Platform::Object^ e)
{
	_touches = 0 ;
}

void IControls::StackView::IStackItem::StackItem_StoryboardFullCompleted(Platform::Object^ sender, Platform::Object^ e)
{
	this->_touches = 0 ;

	if(_ismfull)
	{
		this->_scalexanimation1->To = this->_maxscale ;
		this->_scaleyanimation1->To = this->_maxscale ;
		//_borderimage->Opacity = 0.0;
		this->_scalestory1->Begin();
			_ismfull = false ;
	}
	else
	{
		if(_transform->ScaleX > 1.0)
		{
			this->ZIndex = this->_maxthreshold ;
			this->_isfull =  true ;
			StackItemFullAnimationCompleted(this , _chapter , _section , _itemnumber);
		}
		else
		{
			this->ZIndex = this->_maxthreshold - this->_itemnumber ;
			this->_isfull =  false ;
			StackItemThumbAnimationCompleted(this , _chapter , _section , _itemnumber);
			DeleteFullSource();
			//_borderimage->Opacity = 1.0;
		}
	}
}

#pragma endregion

#pragma region Animations 
			
void IStackItem::inititemanimations()
{
	Windows::Foundation::TimeSpan ts;
	ts.Duration = 3500000 ;
	Windows::UI::Xaml::Duration dur(ts) ;

	this->_translatestory = ref new Windows::UI::Xaml::Media::Animation::Storyboard();
	this->_translateXanimation = ref new Windows::UI::Xaml::Media::Animation::DoubleAnimation();
	this->_translateXanimation->Duration = dur;
	this->_translatestory->Children->Append(this->_translateXanimation);
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTarget(this->_translateXanimation, this->_transform);
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTargetProperty(this->_translateXanimation, "TranslateX");
	Windows::UI::Xaml::Media::Animation::QuinticEase ^ ease1 = ref new Windows::UI::Xaml::Media::Animation::QuinticEase();
	ease1->EasingMode = Windows::UI::Xaml::Media::Animation::EasingMode::EaseOut;
	this->_translateXanimation->EasingFunction = ease1;

	this->_rotatestory =  ref new Windows::UI::Xaml::Media::Animation::Storyboard();
	this->_rotateanimation = ref new Windows::UI::Xaml::Media::Animation::DoubleAnimation(); ;	
	this->_rotateanimation->Duration = dur ;
	this->_rotatestory->Children->Append(_rotateanimation) ;	
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTarget(this->_rotateanimation, this->_transform) ;
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTargetProperty(this->_rotateanimation , "Rotation") ; 
	this->_translatestory->Completed += ref new EventHandler<Platform::Object^>(this, &IControls::StackView::IStackItem::StackItem_StoryboardCompleted);

	///to full
	Windows::Foundation::TimeSpan ts1;
	ts1.Duration = 4000000; //1750000 ;
	Windows::UI::Xaml::Duration duration(ts1) ;

	_translatestory1 = ref new Windows::UI::Xaml::Media::Animation::Storyboard() ;
	_translatexanimation1 = ref new Windows::UI::Xaml::Media::Animation::DoubleAnimation() ;
	this->_translatexanimation1->Duration = duration ; 
	this->_translatestory1->Children->Append(this->_translatexanimation1);
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTargetProperty(_translatexanimation1 , "TranslateX");
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTarget(_translatexanimation1 , _transform);

	_translateyanimation1 = ref new Windows::UI::Xaml::Media::Animation::DoubleAnimation() ;
	this->_translateyanimation1->Duration = duration ; 
	this->_translatestory1->Children->Append(this->_translateyanimation1);
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTargetProperty(_translateyanimation1 , "TranslateY");
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTarget(_translateyanimation1 , _transform);

	_scalestory1 = ref new Windows::UI::Xaml::Media::Animation::Storyboard() ;
	_scalexanimation1 = ref new Windows::UI::Xaml::Media::Animation::DoubleAnimation();
	this->_scalexanimation1->Duration = duration ; 
	this->_scalestory1->Children->Append(this->_scalexanimation1);
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTargetProperty(_scalexanimation1 , "ScaleX");
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTarget(_scalexanimation1 , _transform);

	Windows::UI::Xaml::Media::Animation::QuinticEase ^ easex =  ref new Windows::UI::Xaml::Media::Animation::QuinticEase();
	easex->EasingMode = Windows::UI::Xaml::Media::Animation::EasingMode::EaseInOut ;
	//this->_scalexanimation1->EasingFunction = easex ;

	_scaleyanimation1 = ref new Windows::UI::Xaml::Media::Animation::DoubleAnimation();
	this->_scaleyanimation1->Duration = duration ; 
	this->_scalestory1->Children->Append(this->_scaleyanimation1);
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTargetProperty(_scaleyanimation1 , "ScaleY");
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTarget(_scaleyanimation1 , _transform);
	Windows::UI::Xaml::Media::Animation::QuinticEase ^ easey =  ref new Windows::UI::Xaml::Media::Animation::QuinticEase();
	easey->EasingMode = Windows::UI::Xaml::Media::Animation::EasingMode::EaseInOut ;
	//this->_scaleyanimation1->EasingFunction = easey ;
	
	_rotatestory1 = ref new Windows::UI::Xaml::Media::Animation::Storyboard();
	_rotateanimation1 = ref new Windows::UI::Xaml::Media::Animation::DoubleAnimation();
	this->_rotateanimation1->Duration = duration ; 
	this->_rotatestory1->Children->Append(this->_rotateanimation1);
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTargetProperty(_rotateanimation1 , "Rotation");
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTarget(_rotateanimation1 , _transform);

	this->_scalestory1->Completed += ref new EventHandler<Platform::Object^>(this, &IControls::StackView::IStackItem::StackItem_StoryboardFullCompleted);

}			

#pragma endregion