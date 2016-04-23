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
using namespace Windows::UI::Xaml::Navigation;

IStackView::IStackView()
{
	initproperties();
	initcontrols();
	initanimationproperties();
}
 

#pragma region Controls 

void IStackView::initcontrols()
{
	this->HorizontalAlignment = Windows::UI::Xaml::HorizontalAlignment::Left ; 
//	this->Background = ref new SolidColorBrush(Windows::UI::Colors::SaddleBrown);
	this->_itemspanel  = ref new StackPanel();
	this->Children->Append(this->_itemspanel);
	this->_itemspanel->Orientation = Orientation::Horizontal ; 

	this->_paneltransform =  ref new CompositeTransform();
	this->_itemspanel->RenderTransform = this->_paneltransform ;

	this->_begingrid =  ref new Grid();
	_itemspanel->Children->Append(_begingrid);
	this->_itemsgrid =  ref new Grid();
	this->_itemsgrid->Background =  ref new SolidColorBrush(Windows::UI::Colors::Transparent);
	_itemspanel->Children->Append(_itemsgrid);
	this->_endgrid =  ref new Grid() ;
	_itemspanel->Children->Append(_endgrid);

	this->_itemspanel->PointerPressed +=  ref new PointerEventHandler(this, &IControls::StackView::IStackView::ItemsPanel_PointerPressed_1);
	this->_itemspanel->PointerReleased +=  ref new PointerEventHandler(this, &IControls::StackView::IStackView::ItemsPanel_PointerReleased_1);
	this->_itemspanel->Tapped += ref new TappedEventHandler(this, &IControls::StackView::IStackView::ItemsPanel_Tapped_1); 

}
#pragma endregion

#pragma region Public Methods
		
void IStackView::LoadDataSource()
{
	for (int i = 1; i < (int)_itemsvector.size(); i++)
		_itemsvector[i]->LoadThumbSource();
}

void IStackView::LoadFirst()
{
	_itemsvector[0]->LoadThumbSource();
	_itemsvector[0]->LoadBorder();
}

void IStackView::LoadBorder()
{
	for (int i = 1; i < (int)_itemsvector.size(); i++)
		_itemsvector[i]->LoadBorder();
}

void IStackView::LoadSeparationItem()
{
}

void IStackView::StackManipulationCompleted()
{  
	if(_selectiontype == SelectionType::StackType)
		if(_proportion > _numberofitems / 2)
			openstack();
		else
			closestack(); 
}

void IStackView::SetToOpen()
{
	this->_itemsgrid->Width = this->_numberofitems * (this->_borderwidth + this->_spacebetweenitems);
	this->Width = this->_numberofitems * (this->_borderwidth + this->_spacebetweenitems) + 2 * _auxgridwidth;
	for (int i = 0; i < (int)_itemsvector.size(); i++)
		_itemsvector[i]->SetToOpen();
	this->_isopen =  true ;
	this->_proportion = this->_numberofitems ;
	this->_currentstackwidth = 2 * _auxgridwidth +  this->_itemsgrid->Width ; 
	_selectiontype =SelectionType::StackType ;
	Canvas::SetZIndex(this, 0);
}
	
void IStackView::SetToClose()
{
	this->_itemsgrid->Width = this->_borderwidth + this->_spacebetweenitems ;
	this->Width = this->_borderwidth + this->_spacebetweenitems + 2 * _auxgridwidth;
	for (int i = 0; i < (int)_itemsvector.size(); i++)
		_itemsvector[i]->SetToClose();

	this->_isopen =  false ;
	this->_proportion = 1.0 ;
	this->_currentstackwidth = 2 * _auxgridwidth +  this->_itemsgrid->Width ;
	_selectiontype = SelectionType::StackType ; 
	Canvas::SetZIndex(this, 0);
}

float64 IStackView::GetItemPosition(int32 page)
{//only if is open
	return (_auxgridwidth / 2) + _itemsvector[page]->FinalPosition ;
}

void IStackView::SetItemToFull(int32 page)
{
	for (int i = 0; i < (int)_itemsvector.size(); i++)
		_itemsvector[i]->ZIndex = 100 - i ; 
	_itemsvector[page]->SetToFull();
	this->_selectedindex = page ;
	this->_selecteditem = _itemsvector[page] ;
	Canvas::SetZIndex(this, 10);
	
}

#pragma endregion


#pragma region Private Methods

void IStackView::initproperties()
{
	_stacknumber = 0 ;
	_numberofitems = 0 ;
	_minstackwidth = 0.0 ;
	_currentstackwidth = 0.0 ;
	_spacebetweenitems = 0.0 ;

	//auxiliar variables
	_touches = 0 ;
	_auxgridwidth = 0.0 ;
	_isopen =  false ;
	_angles[0] = 0.0 ;
	_angles[1] = 8.0 ;
	_angles[2] = 15.0 ;

	_selectiontype = SelectionType::None ;
	_proportion = 1.0 ;
	_itemslocked = false ; 
}

void IStackView::initcomponent()
{
	if(_datasource != nullptr)
	{
		_numberofitems = _datasource->Pages->Size ;
		//_numberofitems = 6;
		for (int i = 0; i < _numberofitems; i++)
		{
			IStackItem ^ sitem =  ref new IStackItem();
			sitem->ItemNumber = i ;
			sitem->InitialAngle = _angles[i%3] ;
			sitem->ThumbHeight = _thumbheight ;
			sitem->ThumbWidth = _thumbwidth ;
			sitem->BorderWidth = _borderwidth ;
			sitem->BorderHeight = _borderheight ;
			sitem->MaxScale =_maxscale ;
			sitem->InitialPosition = _spacebetweenitems / 2 ;
			sitem->FinalPosition = _borderwidth * i  + _spacebetweenitems * (2*i + 1) / 2 ;
			sitem->BorderSource = _bordersource ;
			sitem->FullPositionX = 0.0 ;
			sitem->FullPositionY = -1 * _verticalposition ;
			sitem->Source = _datasource->Pages->GetAt(i);

			sitem->StackItemSelected +=  ref new StackItemSelectedEventHandler(this ,  &IControls::StackView::IStackView::StackItem_Selected);
			sitem->StackItemTapped +=  ref new StackItemTappedEventHandler(this ,  &IControls::StackView::IStackView::StackItem_Tapped); 

			sitem->StackItemFullAnimationStarted +=  ref new StackItemFullAnimationStartedEventHandler(this , &IControls::StackView::IStackView::StackItem_FullAnimationStarted);
			sitem->StackItemFullAnimationCompleted +=  ref new StackItemFullAnimationCompletedEventHandler(this , &IControls::StackView::IStackView::StackItem_FullAnimationCompleted);
			sitem->StackItemThumbAnimationStarted +=  ref new StackItemThumbAnimationStartedEventHandler(this , &IControls::StackView::IStackView::StackItem_ThumbAnimationStarted);
			sitem->StackItemThumbAnimationCompleted +=  ref new StackItemThumbAnimationCompletedEventHandler(this , &IControls::StackView::IStackView::StackItem_ThumbAnimationCompleted);

			this->_itemsgrid->Children->Append(sitem);
			_itemsvector.push_back(sitem);
		}
		_selecteditem = _itemsvector[0] ;
		this->_currentstackwidth = 2 * _auxgridwidth + this->_borderwidth + this->_spacebetweenitems ;
	}
}

void IStackView::openstack()
{
	//this->_itemgridanimation->From = this->_spacebetweenitems + this->_borderwidth ;

	for (int i = 0; i < (int)_itemsvector.size(); i++)
		_itemsvector[i]->AnimateToOpen();

	this->_itemgridanimation->To = this->_numberofitems * ( this->_spacebetweenitems + this->_borderwidth ) ;
	this->_itemsgridstory->Begin();
	this->_viewanimation->To = 2 * _auxgridwidth + this->_numberofitems * ( this->_spacebetweenitems + this->_borderwidth ) ;
	this->_viewstory->Begin();

	this->_currentstackwidth = 2 * _auxgridwidth +  this->_numberofitems * ( this->_spacebetweenitems + this->_borderwidth ) ;
	StackSizeAnimationStarted(this, true); //true -> animate to open
	this->_isopen =  true ;
	this->_proportion = this->_numberofitems ;	
	_itemslocked = true ;
}

void IStackView::closestack()
{
	//this->_itemgridanimation->From = this->_numberofitems * ( this->_spacebetweenitems + this->_borderwidth ) ;
	for (int i = 0; i < (int)_itemsvector.size(); i++)
		_itemsvector[i]->AnimateToClose();

	this->_itemgridanimation->To =  this->_spacebetweenitems + this->_borderwidth ;
	this->_viewanimation->To = 2 * _auxgridwidth +  this->_spacebetweenitems + this->_borderwidth  ; 
	this->_itemsgridstory->Begin();
	this->_viewstory->Begin();

	this->_currentstackwidth = 2 * _auxgridwidth +  this->_spacebetweenitems + this->_borderwidth  ;
	StackSizeAnimationStarted(this, false);
	_itemslocked = true ;
	this->_isopen =  false ;
	this->_proportion = 1.0 ;
}

void IStackView::updateproportion()
{
	for (int i = 0; i < (int)_itemsvector.size(); i++)
	{
		_itemsvector[i]->ItemTransform->TranslateX = (_proportion - 1 )*(_itemsvector[i]->FinalPosition - _itemsvector[i]->InitialPosition)/(_numberofitems - 1) + _itemsvector[i]->InitialPosition ;
		_itemsvector[i]->ItemTransform->Rotation = (_proportion - 1 ) * _itemsvector[i]->InitialAngle / ( 1 - _numberofitems)  + _itemsvector[i]->InitialAngle ; 
	}
	
	this->_itemsgrid->Width = _proportion * (_borderwidth + _spacebetweenitems) ;
	this->Width =this->_itemsgrid->Width + 2*_auxgridwidth ;

	//Change the size of the stack -> Update the positon of the stack to keep this in this initial position
	if(_isopen)
		StackSizeChangeDelta(this , _tempposition - (_tmpproportion * this->Width));// _tempposition -  _tempindex * (this->_itemsgrid->Width  / _numberofitems)  + _auxgridwidth );
	if (_proportion < _numberofitems)
	{
		for (int i = 0; i < (int)_itemsvector.size(); i++)
			_itemsvector[i]->HiddeText();
	}
}


void IStackView::updatehorizontalposition()
{ 
	float64 center = (_devicewidth  - _auxgridwidth - _borderwidth - (_spacebetweenitems / 2 ))/2 - _distancetoscreen ;
	for (int i = 0; i < (int)_itemsvector.size(); i++)
		_itemsvector[i]->FullPositionX = center ;
	//_itemslocked = false ;
}
#pragma endregion

#pragma region Events Methods

void IControls::StackView::IStackView::StackItem_Selected(Platform::Object ^ sender , int32 _itemnumber)
{
	if(_selectiontype != SelectionType::ItemType && !_itemslocked)
	{
		this->_selecteditem = _itemsvector[ _itemnumber] ;
		this->_selectiontype = SelectionType::ItemType ; 
		this->_selectedindex = _itemnumber ;
		this->_selecteditem->ZIndex = 100  ;
		_selecteditem->LoadFullSource();
		IControlsComponentSelected(this, SelectionType::ItemType, _stacknumber);
		Canvas::SetZIndex(this, 10);
	}

	_itemslocked =true ;
	/**
	else
	{
		_itemsvector[_itemnumber]->ZIndex -= _itemnumber ;
		_itemsvector[_itemnumber]->DeleteFullSource();
	}*/
}

int32 count = 0 ;

void IControls::StackView::IStackView::StackItem_Tapped(Platform::Object ^ sender , int32 _itemnumber)
{
	count++;
	if(count>1)
	{
	int a = 3 ;
	}
	if(!_itemslocked)
	{
		_itemslocked = true ;
		if(_selectiontype != SelectionType::ItemType || _touches < 2)
		{
			this->_selecteditem = _itemsvector[ _itemnumber] ;
			this->_selectiontype = SelectionType::ItemType ; 
			this->_selectedindex = _itemnumber ;
			this->_selecteditem->ZIndex = 100   ;
			_selecteditem->LoadFullSource();
			_selecteditem->AnimateToFull();
			IControlsComponentSelected(this, SelectionType::ItemType , _stacknumber);
			Canvas::SetZIndex(this, 10);
		}
		else
		{
			//_itemsvector[_itemnumber]->ZIndex -= _itemnumber ;
			_itemsvector[_itemnumber]->DeleteFullSource();
		}
	}
		
}

 
	 
void IControls::StackView::IStackView::ItemsPanel_PointerReleased_1(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e)
{
	_touches = 0 ;
}

void IControls::StackView::IStackView::ItemsPanel_PointerPressed_1(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e)
{
	this->_touches +=  1 ;
	if(_touches < 2)
	{
		this->_selectiontype = SelectionType::StackType ;
		_tempindex = (int32)ceil(e->GetCurrentPoint(this->_itemsgrid)->Position.X / (_borderwidth + _spacebetweenitems));
		_tempposition = e->GetCurrentPoint(this)->Position.X  + _thumbwidth ; /// _tempindex * (this->_itemsgrid->Width / _numberofitems) + _auxgridwidth ;
		_tmpproportion = _tempposition / _currentstackwidth ;
		StackSizeChangeStarted(this, _tempposition );
		IControlsComponentSelected(this, SelectionType::StackType ,  _stacknumber);
		Canvas::SetZIndex(this, 10);
		//hidde all text 
	}
}


void IControls::StackView::IStackView::ItemsPanel_Tapped_1(Platform::Object^ sender, Windows::UI::Xaml::Input::TappedRoutedEventArgs^ e)
{
	if(!_isopen)
	{///open the stack
		this->_selectiontype = SelectionType::StackType ;
		openstack();
	}
}


void IControls::StackView::IStackView::StackItem_FullAnimationStarted(Platform::Object ^ sender , int32 chapter , int32 section , int32 page )
{
	StackItemFullAnimationStarted(sender , chapter , _stacknumber , page) ;
	_itemslocked = true ;
}


void IControls::StackView::IStackView::StackItem_FullAnimationCompleted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page  )
{
	StackItemFullAnimationCompleted(sender , chapter , _stacknumber , page ) ;
}


void IControls::StackView::IStackView::StackItem_ThumbAnimationStarted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page )
{
	StackItemThumbAnimationStarted(this , chapter , _stacknumber , page );
	this->_selectiontype = SelectionType::ItemType ;
	_itemslocked = true ;
}


void IControls::StackView::IStackView::StackItem_ThumbAnimationCompleted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page )
{
	StackItemThumbAnimationCompleted(this, chapter , _stacknumber , page );
	this->_selectiontype = SelectionType::StackType ;
	_touches = 0 ;
	_itemslocked = false ;

	count = 0 ; 
}


#pragma endregion

#pragma region Animation 
			
void IStackView::initanimationproperties()
{
	Windows::Foundation::TimeSpan ts;
	ts.Duration = 3500000 ;
	Windows::UI::Xaml::Duration duration(ts) ;

	this->_itemsgridstory =  ref new Windows::UI::Xaml::Media::Animation::Storyboard();
	this->_itemgridanimation = ref new Windows::UI::Xaml::Media::Animation::DoubleAnimation();
	this->_itemgridanimation->Duration = duration ;
	this->_itemgridanimation->EnableDependentAnimation = true ; 
	this->_itemsgridstory->Children->Append(this->_itemgridanimation);
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTargetProperty(_itemgridanimation , "(Grid.Width)");
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTarget(_itemgridanimation , _itemsgrid);

	this->_viewstory =  ref new Windows::UI::Xaml::Media::Animation::Storyboard();
	this->_viewanimation = ref new Windows::UI::Xaml::Media::Animation::DoubleAnimation();
	this->_viewanimation->Duration = duration ;
	this->_viewanimation->EnableDependentAnimation = true ; 
	this->_viewstory->Children->Append(this->_viewanimation);
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTargetProperty(_viewanimation , "(Grid.Width)");
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTarget(_viewanimation , this);

	this->_viewstory->Completed += ref new EventHandler<Platform::Object^>(this, &IControls::StackView::IStackView::Storyboard_Completed_1);
}


void IControls::StackView::IStackView::Storyboard_Completed_1(Platform::Object^ sender, Platform::Object^ e)
{
	StackSizeChangeCompleted(this, 0.0);
	if(_proportion > 1.0)
	{
		StackSizeAnimationCompleted(this, true);
		//show the text of every item stack
		for (int i = 0; i < (int)_itemsvector.size(); i++)
			_itemsvector[i]->ShowText(); 
	}
	else
	{
		StackSizeAnimationCompleted(this, false);
	}
	this->_selectiontype = SelectionType::StackType ;
	_touches = 0 ;
	Canvas::SetZIndex(this, 0);
	_itemslocked = false ;
}

#pragma endregion