#include "pch.h"

using namespace IControls::StackView ;

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

IGroupList::IGroupList()
{
	this->_texto =  ref new TextBlock();
	_texto->FontSize  = 25 ;
	_texto->Width = 200 ;
	_texto->Height = 200 ;
	_texto->VerticalAlignment = Windows::UI::Xaml::VerticalAlignment::Top ;
	_texto->Foreground =  ref new SolidColorBrush(Windows::UI::Colors::Black);
	this->Children->Append(_texto);
	initcontrols();
	initproperties();
	initanimationproperties();
}
 

#pragma region Controls

void IGroupList::initcontrols()
{ 

	this->_grouppanel =  ref new StackPanel(); 
	_grouppanel->Background = ref new SolidColorBrush(Windows::UI::Colors::Transparent);
	this->_grouppanel->Orientation =  Orientation::Horizontal ; 
	this->_grouppanel->ManipulationMode =  ManipulationModes::All ;
	this->_grouppanel->ManipulationDelta += ref new ManipulationDeltaEventHandler(this, & IControls::StackView::IGroupList::Panel_ManipulationDelta_1);
	this->_grouppanel->ManipulationCompleted += ref new ManipulationCompletedEventHandler(this, & IControls::StackView::IGroupList::Panel_ManipulationCompleted_1);
	this->_grouppanel->ManipulationInertiaStarting += ref new ManipulationInertiaStartingEventHandler(this, & IControls::StackView::IGroupList::Panel_ManipulationInertiaStarting_1);
	this->_grouppanel->PointerPressed += ref new PointerEventHandler(this, &IControls::StackView::IGroupList::Panel_PointerPressed_1);
	this->_grouppanel->PointerReleased += ref new PointerEventHandler(this, &IControls::StackView::IGroupList::Panel_PointerReleased_1);

	this->_groupscroll = ref new ScrollViewer();
	this->_groupscroll->Background = ref new SolidColorBrush(Windows::UI::Colors::Transparent);
	this->_groupscroll->HorizontalScrollMode = Windows::UI::Xaml::Controls::ScrollMode::Disabled;
	this->_groupscroll->VerticalScrollMode =  Windows::UI::Xaml::Controls::ScrollMode::Disabled;
	this->_groupscroll->HorizontalScrollBarVisibility = Windows::UI::Xaml::Controls::ScrollBarVisibility::Hidden ;
	this->_groupscroll->VerticalScrollBarVisibility = Windows::UI::Xaml::Controls::ScrollBarVisibility::Hidden ;
	this->_groupscroll->ZoomMode = ZoomMode::Disabled ;
	this->_groupscroll->IsHorizontalRailEnabled  = false ;
	this->Children->Append(this->_groupscroll);
	this->_groupscroll->Content = this->_grouppanel ;

	this->_paneltransform = ref new CompositeTransform();
	this->_grouppanel->RenderTransform =  this->_paneltransform ;


}
			
#pragma endregion

#pragma region Public Methods

void IGroupList::LoadList(int32 number)
{
	if(!_listvector[number]->IsLoaded)
		_listvector[number]->LoadDataSource();
}

void IGroupList::SetToItem(int32 chapter, int32 section, int32 page)
{
	this->_selectedchapter = chapter ;
	this->_selectedsection = section ;
	this->_selectedpage = page ;
	//reset stacks
	for (int i = 0; i < (int)_listvector.size(); i++)
		if( i == chapter)
			_listvector[i]->OpenStack(section);
		else
			_listvector[i]->OpenStack(0);
	
	updatelistproperties(); 
	computethresholds();
	//get item position
	float64 itempos = _listvector[chapter]->Position + _listvector[chapter]->GetItemPosition(section, page);
	float64 pos = -1 * ( itempos - _controlwidth/2 + _borderwidth/2) ;
	if(pos > _initthreshold)
		pos = _initthreshold ;
	if(pos < _finalthreshold)
		pos = _finalthreshold ;
	this->_paneltranslate = pos  ;
	this->_paneltransform->TranslateX = _paneltranslate ;
	updatelistproperties();
	
	//set item to full
	_listvector[chapter]->SetItemToFull(section , page);
}


void IGroupList::AnimateToChapter(int32 chapter)
{
	animatetochapter(chapter, true);
}


void IGroupList::TranslateTo(float64 value)
{
	_paneltransform->TranslateX = _paneltranslate + value* _leftconstant;
}

#pragma endregion

#pragma region Private Methods
		
void IGroupList::initproperties()
{
	_maxscale = 6.0;
	//_bordersource = IControls::FrameSource;
	_thumbheight = IControls::ThumbHeight;
	_thumbwidth = IControls::ThumbWidth;
	_borderheight = IControls::FrameHeight;
	_borderwidth = IControls::FrameWidth;
	_verticalposition = 0.0;/////////300.0 ;
	_minstackwidth = 347.0;
	_spacebetweenitems = 20.0;

	_maxscale = IControls::DeviceWidth / _thumbwidth;

	this->_controlwidth = IControls::DeviceWidth;
	this->_controlheight = IControls::DeviceHeight;
	this->_startindex = 0;
	_selectedchapter = 0;
	_selectedsection = 0;
	_selectedpage = 0;
	_typeselected = SelectionType::StackType;
	_manipulationenable = true;
	_forcemanipulationtoend = false;
	_isinertia = false;
	_ismanipulating = false;
	_isitemselected = false;
	_touches = 0;
	_offsetdelta = 0;
}

void IGroupList::loadcontrols()
{
	if(_datasource != nullptr)
	{
		Background = ref new SolidColorBrush(Windows::UI::Colors::Transparent);
		this->Width = _controlwidth;
		this->Height = _controlheight;

		if (_datasource->Chapters != nullptr){

			this->_numberofitems = _datasource->Chapters->Size;
			for (int i = 0; i < _numberofitems; i++)
			{
				IStackList ^ list = ref new IStackList();
				list->Chapter = i;
				list->ListNumber = i;
				list->MaxScale = _maxscale;
				list->BorderSource = _bordersource;
				list->ThumbHeight = _thumbheight;
				list->ThumbWidth = _thumbwidth;
				list->BorderHeight = _borderheight;
				list->BorderWidth = _borderwidth;
				list->StackVerticalPosition = _verticalposition;
				list->MinStackWidth = _minstackwidth;
				list->SpaceBetweenItems = _spacebetweenitems;

				list->MinListWidth = _controlwidth;
				list->ListHeight = _controlheight;
				list->Source = _datasource->Chapters->GetAt(i);

				list->StackItemFullAnimationStarted += ref new StackItemFullAnimationStartedEventHandler(this, &IControls::StackView::IGroupList::StackItem_FullAnimationStarted);
				list->StackItemFullAnimationCompleted += ref new StackItemFullAnimationCompletedEventHandler(this, &IControls::StackView::IGroupList::StackItem_FullAnimationCompleted);
				list->StackItemThumbAnimationStarted += ref new StackItemThumbAnimationStartedEventHandler(this, &IControls::StackView::IGroupList::StackItem_ThumbAnimationStarted);
				list->StackItemThumbAnimationCompleted += ref new StackItemThumbAnimationCompletedEventHandler(this, &IControls::StackView::IGroupList::StackItem_ThumbAnimationCompleted);

				list->StackListAnimateTo += ref new StackListAnimateToEventHandler(this, &IControls::StackView::IGroupList::StackList_AnimateTo);
				list->StackListScrollTo += ref new StackListScrollToEventHandler(this, &IControls::StackView::IGroupList::StackList_ScrollTo);
				list->StackListWidthChanged += ref new StackListWidthChangedEventHandler(this, &IControls::StackView::IGroupList::StackList_WidthChanged);
				list->IControlsComponentSelected += ref new IControlsComponentSelectedEventHandler(this, &IControls::StackView::IGroupList::IControls_ComponentSelected);
				this->_grouppanel->Children->Append(list);
				_listvector.push_back(list);
			}
			LoadList(_startindex);
			///open the fisrt the stacks
			//for (int i = 0; i < (int)_listvector.size(); i++) 
			//_listvector[i]->OpenStack(0);

			_selectedchapter = _startindex;
			updatelistproperties();
			computethresholds();

			for (int i = 0; i < _numberofitems; i++)
				LoadList(i);

			_texto->Text = "" + _selectedchapter;

			_datasource->Chapters->VectorChanged += ref new Windows::Foundation::Collections::VectorChangedEventHandler<IControls::DataSource::ChapterDataSource ^>(this, &IControls::StackView::IGroupList::chaptersVectorChanged_1);
		}
	}
}


void IControls::StackView::IGroupList::chaptersVectorChanged_1(Windows::Foundation::Collections::IObservableVector<IControls::DataSource::ChapterDataSource ^> ^sender, Windows::Foundation::Collections::IVectorChangedEventArgs ^event)
{
	this->_numberofitems = _datasource->Chapters->Size;
	int32 sindex = event->Index;
	for (int i = sindex; i < _numberofitems; i++)
	{
		IStackList ^ list = ref new IStackList();
		list->Chapter = i;
		list->ListNumber = i;
		list->MaxScale = _maxscale;
		list->BorderSource = _bordersource;
		list->ThumbHeight = _thumbheight;
		list->ThumbWidth = _thumbwidth;
		list->BorderHeight = _borderheight;
		list->BorderWidth = _borderwidth;
		list->StackVerticalPosition = _verticalposition;
		list->MinStackWidth = _minstackwidth;
		list->SpaceBetweenItems = _spacebetweenitems;

		list->MinListWidth = _controlwidth;
		list->ListHeight = _controlheight;
		list->Source = _datasource->Chapters->GetAt(i);

		list->StackItemFullAnimationStarted += ref new StackItemFullAnimationStartedEventHandler(this, &IControls::StackView::IGroupList::StackItem_FullAnimationStarted);
		list->StackItemFullAnimationCompleted += ref new StackItemFullAnimationCompletedEventHandler(this, &IControls::StackView::IGroupList::StackItem_FullAnimationCompleted);
		list->StackItemThumbAnimationStarted += ref new StackItemThumbAnimationStartedEventHandler(this, &IControls::StackView::IGroupList::StackItem_ThumbAnimationStarted);
		list->StackItemThumbAnimationCompleted += ref new StackItemThumbAnimationCompletedEventHandler(this, &IControls::StackView::IGroupList::StackItem_ThumbAnimationCompleted);

		list->StackListAnimateTo += ref new StackListAnimateToEventHandler(this, &IControls::StackView::IGroupList::StackList_AnimateTo);
		list->StackListScrollTo += ref new StackListScrollToEventHandler(this, &IControls::StackView::IGroupList::StackList_ScrollTo);
		list->StackListWidthChanged += ref new StackListWidthChangedEventHandler(this, &IControls::StackView::IGroupList::StackList_WidthChanged);
		list->IControlsComponentSelected += ref new IControlsComponentSelectedEventHandler(this, &IControls::StackView::IGroupList::IControls_ComponentSelected);
		this->_grouppanel->Children->Append(list);
		_listvector.push_back(list);
	}
	LoadList(_startindex);
	///open the fisrt the stacks
	//for (int i = 0; i < (int)_listvector.size(); i++) 
	//_listvector[i]->OpenStack(0);

	_selectedchapter = _startindex;
	updatelistproperties();
	computethresholds();

	for (int i = sindex; i < _numberofitems; i++)
		LoadList(i);

	_texto->Text = "" + _selectedchapter;

}



void IGroupList::computethresholds()
{
	/**paged scroll view*/
	_initthreshold = -1 * _listvector[_selectedchapter]->Position ;
	_finalthreshold = _initthreshold - _listvector[_selectedchapter]->CurrentListWidth + _controlwidth; 
	

	/** infite scroll 
	_initthreshold = -1 * 0.0;
	_finalthreshold = _initthreshold;
	for (size_t j = 0; j < _listvector.size(); j++)
		_finalthreshold -= _listvector[j]->CurrentListWidth;

	_finalthreshold += _controlwidth; 

	end final scroll*/

	if(_finalthreshold > _initthreshold)
		_finalthreshold = _initthreshold ;

	_leftconstant = _listvector[_selectedchapter]->CurrentListWidth / 1600.0;
}

//pos >= 0
void IGroupList::animatetoposition(float64 pos)
{
	float64 to = -1 * pos ;
	if(to > _initthreshold)
		to = _initthreshold ;

	if(to < _finalthreshold )
		to = _finalthreshold ; 
	this->_panelanimation->To = to ;
	this->_panelstory->Begin();
}

void IGroupList::animatetochapter(int32 chapter, bool tobegin)
{
	int32 ch = chapter ;
	bool tb = tobegin ;
	if(chapter<0)
		ch = 0 ;
	if(chapter>=_numberofitems)
	{
		ch = _numberofitems - 1 ;
		tb = false ;
	} 
	StackListScrollCompleted(this, ch);
	_selectedchapter = ch ;
	if(tb)
	{
		this->_panelanimation->To = -1 * _listvector[_selectedchapter]->Position  ;
		this->_panelstory->Begin(); 
	}
	else
	{
		this->_panelanimation->To = -1 * (_listvector[_selectedchapter]->Position + _listvector[_selectedchapter]->CurrentListWidth - _controlwidth) ;
		this->_panelstory->Begin(); 
	}		

	_texto->Text = "" + _selectedchapter ;
}

void IGroupList::updatelistproperties()
{
	float64 tempos = 0.0 ;
	for (int i = 0; i < (int)_listvector.size(); i++)
	{
		_listvector[i]->Position = tempos ;
		_listvector[i]->DistanceToScreen = _paneltransform->TranslateX + tempos;
		_listvector[i]->IsManipulating = false ;
		_listvector[i]->UpDateProperties();

		if(tempos<0)
		{
			float64 a = _listvector[i]->CurrentListWidth ;
			float64 b = _listvector[i]->Position ;
		}
		tempos += _listvector[i]->CurrentListWidth ;
	}

	_texto->Text = "" + _selectedchapter ;
}

#pragma endregion

#pragma region Manipulation Events Functions
		
void IControls::StackView::IGroupList::Panel_PointerReleased_1(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e)
{
	if(_ismanipulating && !_isinertia)
	{
		_forcemanipulationtoend= true ;
	}
	
	if(!_isitemselected)
	{	
		_touches = 0 ;
		updatelistproperties();
	}
}

void IControls::StackView::IGroupList::Panel_PointerPressed_1(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e)
{
	this->_touches += 1 ;
}



void IControls::StackView::IGroupList::Panel_ManipulationDelta_1(Platform::Object^ sender, Windows::UI::Xaml::Input::ManipulationDeltaRoutedEventArgs^ e)
{
	if(_manipulationenable)
	{
		if(_touches < 2 )
		{
			if(_paneltranslate<_initthreshold && _paneltranslate>_finalthreshold )
			{
				_paneltranslate += e->Delta.Translation.X ;
				_offsetdelta =  0.0 ;
				//compute chapter while scrolling
				if (-1.0*_paneltranslate < _listvector[_selectedchapter]->Position)
				{ 
					_selectedchapter -= 1;
					if (_selectedchapter < 0)
						_selectedchapter = 0;
					StackListScrollCompleted(this, _selectedchapter);
				}
				else
				{
					if (_selectedchapter < _listvector.size() - 1)
						if (-1.0*_paneltranslate > _listvector[_selectedchapter + 1]->Position)
						{
						_selectedchapter += 1;
						StackListScrollCompleted(this, _selectedchapter);
						}
				}
				
			}
			else
			{
				if(e->IsInertial)
				{
					e->Complete();
					return ;
				}
				_paneltranslate += (e->Delta.Translation.X * 0.7 ) ; //0.5
				_offsetdelta = (e->Delta.Translation.X * 0.7 ) ; //0.5
				//Scroll UPControl
				StackListScrollDelta(this , _offsetdelta); 
			}
			this->_paneltransform->TranslateX = _paneltranslate ;
		}
		else
		{
			_isitemselected = true ;
			if(_typeselected ==  SelectionType::ItemType)
			{
				_listvector[_selectedchapter]->SelectedStack->SelectedStackItem->ItemTransform->TranslateX += e->Delta.Translation.X ;
				_listvector[_selectedchapter]->SelectedStack->SelectedStackItem->ItemTransform->TranslateY += e->Delta.Translation.Y ;
				_listvector[_selectedchapter]->SelectedStack->SelectedStackItem->ItemTransform->ScaleX *= e->Delta.Scale ;
				_listvector[_selectedchapter]->SelectedStack->SelectedStackItem->ItemTransform->ScaleY *= e->Delta.Scale ;
				_listvector[_selectedchapter]->SelectedStack->SelectedStackItem->ItemTransform->Rotation += e->Delta.Rotation ;
			}
			else{
				if(_typeselected == SelectionType::StackType)
				{
					_listvector[_selectedchapter]->SelectedStack->Proportion *= e->Delta.Scale ;
				}
			}
		}
		_ismanipulating =  true ;
		_listvector[_selectedchapter]->IsManipulating  = true ;
	}
	else
	{
		e->Complete();
		e->Handled =  true ;
		return ;
	}

	if(_forcemanipulationtoend && !e->IsInertial)
	{
		e->Complete();
		return;
	}
}

void IControls::StackView::IGroupList::Panel_ManipulationCompleted_1(Platform::Object^ sender, Windows::UI::Xaml::Input::ManipulationCompletedRoutedEventArgs^ e)
{
	if(_manipulationenable)
	{
		if(_touches < 2 || !_isitemselected)
		{
			int32 tchapter = _selectedchapter ;
			bool tobegin = true ;
			if(_paneltranslate>_initthreshold )
			{				
				if(_paneltranslate>_initthreshold + _borderwidth)
					tchapter -=  1 ; 
				//paged scroll
				animatetochapter(tchapter, tobegin);

				//infite scroll
				//animatetochapter(0, true);
			}else
			{
				if(_paneltranslate<_finalthreshold )
				{
					if(_paneltranslate<_finalthreshold - _borderwidth  )
						tchapter +=  1 ;
					else
						tobegin = false ;
					//paged scroll
					animatetochapter(tchapter, tobegin);

					//infite scroll
					//animatetochapter(_listvector.size() - 1, false);
				}
				else
				{
					updatelistproperties() ;
					this->_paneltranslate = this->_paneltransform->TranslateX ;
					this->_manipulationenable = true ;
					_touches = 0 ;
				}
			}

			_leftconstant = _listvector[_selectedchapter]->CurrentListWidth / 1600.0 ;

//			StackListScrollCompleted(this, tchapter);
		}
		else
		{
			if(_typeselected ==  SelectionType::ItemType)
			{
				_listvector[_selectedchapter]->SelectedStack->SelectedStackItem->ItemManipulationCompleted();
			}
			else
			{
				_listvector[_selectedchapter]->SelectedStack->StackManipulationCompleted();
			}
		}
	}
	_touches = 0 ;
	_offsetdelta =  0.0 ;
	_forcemanipulationtoend = false ;
	_ismanipulating =  false ;
	_isinertia = false ;
	_isitemselected = false ;
}

void IControls::StackView::IGroupList::Panel_ManipulationInertiaStarting_1(Platform::Object^ sender, Windows::UI::Xaml::Input::ManipulationInertiaStartingRoutedEventArgs^ e)
{
	_isinertia = true ;
	if(_manipulationenable)
	{
		if(_touches > 1)
		{
			e->TranslationBehavior->DesiredDeceleration = 300.0 * 96.0 / (1000.0 * 1000.0);
			e->ExpansionBehavior->DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0); 
			e->RotationBehavior->DesiredDeceleration = 300.0 * 96.0 / (1000.0 * 1000.0); 
		} 
	}
}

#pragma endregion

#pragma region Stack Events Functions


void IControls::StackView::IGroupList::StackItem_FullAnimationStarted(Platform::Object ^ sender , int32 chapter , int32 section , int32 page )
{
	_selectedchapter = chapter ;
	_selectedpage =  page ;
	_selectedsection = section ;
	StackItemFullAnimationStarted(this, chapter , section , page);
	this->_manipulationenable = false ;
}

void IControls::StackView::IGroupList::StackItem_FullAnimationCompleted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page  )
{
	StackItemFullAnimationCompleted(this, chapter, section, page);
	SetToItem(chapter , section , page) ;
	this->_manipulationenable = true ;
}

void IControls::StackView::IGroupList::StackItem_ThumbAnimationStarted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page )
{
	this->_manipulationenable = false ;
	this->_selectedchapter = chapter ;
	this->_selectedpage = page ;
	this->_selectedsection = section ;
	StackItemThumbAnimationStarted(this, chapter , section , page);
}

void IControls::StackView::IGroupList::StackItem_ThumbAnimationCompleted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page )
{
	this->_manipulationenable =  true ;
	this->_typeselected = SelectionType::StackType ;
	StackItemThumbAnimationCompleted(this, chapter , section , page) ;
}

void IControls::StackView::IGroupList::StackList_ScrollTo(Platform::Object^ sender, float64 _position)
{
	float64 pos =-1*( _listvector[_selectedchapter]->Position + _position) ;
	if(pos > _initthreshold)
		pos = _initthreshold ;
	if(pos < _finalthreshold)
		pos = _finalthreshold ;
	_paneltranslate =  pos ; 
	this->_paneltransform->TranslateX =  pos ; 
}

void IControls::StackView::IGroupList::StackList_AnimateTo(Platform::Object^ sender, float64 _position)
{
	computethresholds();
	float64 p = _listvector[_selectedchapter]->Position ;
	animatetoposition(_listvector[_selectedchapter]->Position +  _position );
}

void IControls::StackView::IGroupList::StackList_WidthChanged(Platform::Object^ sender, float64 _position)
{

}

void IControls::StackView::IGroupList::IControls_ComponentSelected(Platform::Object ^ sender, IControls::SelectionType t , int32 index)
{
	this->_selectedchapter = index ;
	this->_typeselected = _listvector[index]->TypeSelected ;
	this->_selectedsection = _listvector[index]->SelectedStack->StackNumber ;
	this->_selectedpage = _listvector[index]->SelectedStack->SelectedStackItem->ItemNumber ;
}

#pragma endregion

#pragma region Animations

void IGroupList::initanimationproperties()
{
	Windows::Foundation::TimeSpan ts;
	ts.Duration = 10000000 ;
	Windows::UI::Xaml::Duration duration(ts) ;

	this->_panelstory =  ref new Windows::UI::Xaml::Media::Animation::Storyboard();
	this->_panelanimation = ref new Windows::UI::Xaml::Media::Animation::DoubleAnimation();
	this->_panelanimation->Duration = duration ;
	this->_panelstory->Children->Append(this->_panelanimation);
	Windows::UI::Xaml::Media::Animation::CubicEase ^ ease1 = ref new Windows::UI::Xaml::Media::Animation::CubicEase();
	ease1->EasingMode = Windows::UI::Xaml::Media::Animation::EasingMode::EaseOut ;
	this->_panelanimation->EasingFunction = ease1 ;
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTargetProperty(_panelanimation , "TranslateX");
	Windows::UI::Xaml::Media::Animation::Storyboard::SetTarget(_panelanimation , _paneltransform); 
	this->_panelstory->Completed += ref new EventHandler<Platform::Object^>(this, &IControls::StackView::IGroupList::Storyboard_Completed_1);
}

void IControls::StackView::IGroupList::Storyboard_Completed_1(Platform::Object^ sender, Platform::Object^ e)
{
	this->_paneltranslate = this->_paneltransform->TranslateX ;
	updatelistproperties();
	computethresholds();
	this->_manipulationenable = true ;
	_touches = 0  ;
}

#pragma endregion