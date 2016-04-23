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
using namespace Windows::UI::Xaml::Media::Imaging;


IStackList::IStackList()
{
	initcontrols();
	initproperties();
	//Background = ref new SolidColorBrush(Windows::UI::Colors::Blue);
}
 

#pragma region Controls

void IStackList::initcontrols()
{
	this->_panel =  ref new StackPanel();
	this->_panel->Orientation = Orientation::Horizontal ;
	this->Children->Append(this->_panel);

	this->_auxgrid = ref new Grid();
	this->_auxgrid->Background = ref new SolidColorBrush(Windows::UI::Colors::Transparent);
	this->_auxgrid->ManipulationMode = ManipulationModes::TranslateX;

	this->_startgrid = ref new Grid();
	this->_startgrid->Height = STARTWIDTH;
	this->_startgrid->Width = STARTWIDTH;
	_panel->Children->Append(_startgrid);

	_headercontrol = ref new Components::ChapterHeaderControl();
	_headercontrol->HorizontalAlignment = Windows::UI::Xaml::HorizontalAlignment::Right;
	_startgrid->Children->Append(_headercontrol);

	/*_startimage = ref new Image();
	_startimage->Width = 376;
	_startimage->Height = 376;
	_startimage->HorizontalAlignment = Windows::UI::Xaml::HorizontalAlignment::Right;
	_startgrid->Children->Append(_startimage);*/

}

#pragma endregion

#pragma region Public Methods

void IStackList::LoadDataSource()
{
	_isloaded =  true ;
	for (int i = 0; i < (int)_stacksvector.size(); i++)
		_stacksvector[i]->LoadFirst();
	for (int i = 0; i < (int)_stacksvector.size(); i++)
		_stacksvector[i]->LoadBorder();
	for (int i = 0; i < (int)_stacksvector.size(); i++)
		_stacksvector[i]->LoadDataSource();
}

void IStackList::UpDateProperties()
{
	updatewidth();
	this->_selectiontype = SelectionType::StackType ; 
}

void IStackList::OpenStack(int32 index)
{
	for (int i = 0; i < (int)_stacksvector.size(); i++)
		if(_stacksvector[i]->IsOpen)
			_stacksvector[i]->SetToClose();
	
	_stacksvector[index]->SetToOpen();
	updatewidth();
	_selectedstack = _stacksvector[index] ;
	_selectedindex = index ;
}

float64 IStackList::GetItemPosition(int32 section, int32 page)
{
	return _stacksvector[section]->Position + _stacksvector[section]->GetItemPosition(page); ;
}

void IStackList::SetItemToFull(int32 section, int32 page)
{
	_stacksvector[section]->SetItemToFull(page);
	this->_selectedindex = section ;
	this->_selectedstack = _stacksvector[section] ;
}

#pragma endregion

#pragma region Private Methods

void IStackList::loadcontrols()
{
	if(_datasource != nullptr)
	{
		this->_numberofstacks = _datasource->Sections->Size ;
		for (int i = 0; i < _numberofstacks; i++)
		{
			IStackView ^ stack = ref new IStackView();
			stack->StackNumber = i ;
			stack->Section = i ;
			stack->BorderSource = _bordersource ;
			stack->ThumbHeight = _thumbheight ;
			stack->ThumbWidth = _thumbwidth ;
			stack->BorderHeight = _borderheight ;
			stack->BorderWidth = _borderwidth ;
			stack->MaxScale = _maxscale ;
			stack->StackHeight = _listheight ;
			stack->VerticalPosition = _verticalposition ;
			stack->MinStackWidth = _minstackwidth ;
			stack->SpaceBetweenItems = _spacebetweenitems ;
			stack->DeviceWidth = _minlistwidth ;
			stack->Source = _datasource->Sections->GetAt(i);

			stack->StackItemFullAnimationStarted +=  ref new StackItemFullAnimationStartedEventHandler(this , &IControls::StackView::IStackList::StackItem_FullAnimationStarted);
			stack->StackItemFullAnimationCompleted +=  ref new StackItemFullAnimationCompletedEventHandler(this , &IControls::StackView::IStackList::StackItem_FullAnimationCompleted);
			stack->StackItemThumbAnimationStarted +=  ref new StackItemThumbAnimationStartedEventHandler(this , &IControls::StackView::IStackList::StackItem_ThumbAnimationStarted);
			stack->StackItemThumbAnimationCompleted +=  ref new StackItemThumbAnimationCompletedEventHandler(this , &IControls::StackView::IStackList::StackItem_ThumbAnimationCompleted);

			stack->StackSizeChangeStarted += ref new StackSizeChangeStartedEventHandler(this , &IControls::StackView::IStackList::Stack_SizeChangeStarted );
			stack->StackSizeChangeDelta += ref new StackSizeChangeDeltaEventHandler(this , &IControls::StackView::IStackList::Stack_SizeChangeDelta );
			stack->StackSizeChangeCompleted += ref new StackSizeChangeCompletedEventHandler(this , &IControls::StackView::IStackList::Stack_SizeChangeCompleted );
			stack->StackSizeAnimationStarted += ref new StackSizeAnimationStartedEventHandler(this , &IControls::StackView::IStackList::Stack_SizeAnimationStarted );
			stack->StackSizeAnimationCompleted += ref new StackSizeAnimationCompletedEventHandler(this , &IControls::StackView::IStackList::Stack_SizeAnimationCompleted );

			stack->IControlsComponentSelected += ref new IControlsComponentSelectedEventHandler(this , &IControls::StackView::IStackList::IControls_ComponentSelected);

			//_startimage->Source = ref new BitmapImage(ref new Uri("ms-appx:///roadsdata/text"+(int)(i+1)+".png"));

			this->_panel->Children->Append(stack);
			_stacksvector.push_back(stack);
		}
		_panel->Children->Append(_auxgrid);
		updatewidth();
	}
}

void IStackList::initproperties()
{
	_isloaded = false ;
	_ismanipulating =  false ;
	_selectiontype = IControls::SelectionType::StackType ;
}

void IStackList::updatewidth()
{
	float64 tempwidth = STARTWIDTH; //0.0 ;
	for (int i = 0; i < (int)_stacksvector.size(); i++)
	{
		_stacksvector[i]->Position = tempwidth ;
		tempwidth += _stacksvector[i]->CurrentWidth ;
	}
	if(tempwidth < _minlistwidth)
	{
		this->_auxgrid->Width = _minlistwidth - tempwidth ;
		this->_currentlistwidth = _minlistwidth ;
		this->Width = _minlistwidth ;
	}
	else
	{
		this->_auxgrid->Width = 0.0 ;
		this->_currentlistwidth = tempwidth ;
		this->Width = tempwidth ;
	}
}


#pragma endregion



#pragma region Events Methods 

void IControls::StackView::IStackList::Stack_SizeChangeStarted(Platform::Object^ sender, float64 pos )
{
	_tempdistoscreen = -1 * this->_distancetoscreen ; 
}

void IControls::StackView::IStackList::Stack_SizeChangeDelta( Platform::Object^ sender, float64 pos )
{
	StackListScrollTo(this , _tempdistoscreen - pos);
}

void IControls::StackView::IStackList::Stack_SizeChangeCompleted(Platform::Object^ sender, float64 pos )
{
}

void IControls::StackView::IStackList::Stack_SizeAnimationStarted(Platform::Object ^sender , bool toopen)
{
	if(toopen)
	{
		updatewidth();
		StackListAnimateTo(this , _stacksvector[_selectedindex]->Position );
	}
	else
	{
		float64 tempanimation = _stacksvector[_selectedindex]->Position - (this->_minstackwidth / 2) - _borderwidth ;
		float64 tempwidth = 0.0 ;
		for (int i = 0; i < (int)_stacksvector.size(); i++)
			tempwidth += _stacksvector[i]->CurrentWidth ;
		this->_currentlistwidth = tempwidth ;
		if((tempanimation + (this->_minstackwidth / 2)) > tempwidth )
			tempanimation = tempwidth - _minstackwidth ; 
		StackListAnimateTo(this , tempanimation );
	}
}

void IControls::StackView::IStackList::Stack_SizeAnimationCompleted(Platform::Object ^sender , bool toopen)
{
	updatewidth();
	_ismanipulating  =  false ;
	IControlsComponentSelected(this, SelectionType::StackType , _listnumber);
	this->_selectedindex = ((IStackView^)sender)->StackNumber ;
}

void IControls::StackView::IStackList::IControls_ComponentSelected(Platform::Object ^ sender, SelectionType t , int32 index)
{
	if(!_ismanipulating)
	{
		this->_selectiontype = t ;
		this->_selectedstack = 	_stacksvector[index] ;
		_selectedindex = index ;
		IControlsComponentSelected(this , t, _listnumber ) ;
	}
}

void IControls::StackView::IStackList::StackItem_FullAnimationStarted(Platform::Object ^ sender , int32 chapter , int32 section , int32 page )
{
	StackItemFullAnimationStarted(sender, _listnumber, section, page);
}

void IControls::StackView::IStackList::StackItem_FullAnimationCompleted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page  )
{
	StackItemFullAnimationCompleted(sender, _listnumber, section, page);
}

void IControls::StackView::IStackList::StackItem_ThumbAnimationStarted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page )
{
	StackItemThumbAnimationStarted(sender, _listnumber, section, page);
}

void IControls::StackView::IStackList::StackItem_ThumbAnimationCompleted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page )
{
	StackItemThumbAnimationCompleted(sender, _listnumber, section, page);
	this->_selectiontype = SelectionType::StackType ;
	_ismanipulating = false ;
}
#pragma endregion