#include "pch.h"

namespace IControls
{
	namespace StackView
	{
		public enum class StackManipulationType
		{
			StackManipulation,
			ItemManipulation,
			NoneManipulation
		};

		public delegate void StackSizeChangeStartedEventHandler(Platform::Object^ sender, float64 pos );
		public delegate void StackSizeChangeDeltaEventHandler( Platform::Object^ sender, float64 pos );
		public delegate void StackSizeChangeCompletedEventHandler(Platform::Object^ sender, float64 pos );
		public delegate void StackSizeAnimationStartedEventHandler(Platform::Object ^sender , bool toopen );
		public delegate void StackSizeAnimationCompletedEventHandler(Platform::Object ^sender , bool toopen );

		public ref class IStackView sealed : public Windows::UI::Xaml::Controls::Grid 
		{
		public:
			IStackView();

			event StackItemFullAnimationStartedEventHandler ^ StackItemFullAnimationStarted ;
			event StackItemFullAnimationCompletedEventHandler ^ StackItemFullAnimationCompleted ;
			event StackItemThumbAnimationStartedEventHandler ^ StackItemThumbAnimationStarted ;
			event StackItemThumbAnimationCompletedEventHandler ^ StackItemThumbAnimationCompleted ;

			event IControlsComponentSelectedEventHandler ^ IControlsComponentSelected  ;

			event StackSizeChangeStartedEventHandler ^ StackSizeChangeStarted ;
			event StackSizeChangeDeltaEventHandler ^ StackSizeChangeDelta ;
			event StackSizeChangeCompletedEventHandler ^ StackSizeChangeCompleted ;
			event StackSizeAnimationStartedEventHandler ^ StackSizeAnimationStarted ;
			event StackSizeAnimationCompletedEventHandler ^ StackSizeAnimationCompleted ;


#pragma region Controls

		private:
			Windows::UI::Xaml::Controls::StackPanel^ _itemspanel ;
			Windows::UI::Xaml::Controls::Grid ^ _itemsgrid ;			
			Windows::UI::Xaml::Controls::Grid^ _begingrid ;
			Windows::UI::Xaml::Controls::Grid^ _endgrid ;

			Windows::UI::Xaml::Media::CompositeTransform ^ _paneltransform ;

			std::vector<IStackItem^> _itemsvector ;
			void initcontrols();

#pragma endregion

#pragma region Paging Properties
		public:
			property int32 Chapter
			{
				void set(int32 value){ this->_chapter =  value ;}
				int32 get(){ return this->_chapter ;}
			}

			property int32 Section
			{
				void set(int32 value){ this->_section = value ;}
				int32 get(){ return this->_section ;}
			}
			 

		private:
			int32 _chapter, _section  ;
#pragma endregion

#pragma region Stack Item Properties

		public:  

			property Platform::String ^ BorderSource
			{
				void set(Platform::String ^ value){
					this->_bordersource = value ;
					for (int i = 0; i < (int)_itemsvector.size(); i++)
						_itemsvector[i]->BorderSource = value ;
				}
				Platform::String ^ get(){ return nullptr ; }
			} 

			property float64 MaxScale
			{
				void set(float64 value)	{ 
					this->_maxscale = value ;
					for (int i = 0; i < (int)_itemsvector.size(); i++)
						_itemsvector[i]->MaxScale = value ;
				}
				float64 get(){ return 0.0 ;}
			}

			property float64 ThumbHeight
			{
				void set(float64 value)
				{   this->_thumbheight = value ;
					for (int i = 0; i < (int)_itemsvector.size(); i++)
						_itemsvector[i]->ThumbHeight = value ;
				}
				float64 get(){ return 0.0 ;}
			}

			property float64  ThumbWidth
			{
				void set(float64 value)
				{
					this->_thumbwidth = value ;
					for (int i = 0; i < (int)_itemsvector.size(); i++)
						_itemsvector[i]->ThumbWidth = value ;   
				}
				float64 get(){ return 0.0 ;}
			}

			property float64  BorderHeight
			{
				void set(float64 value)
				{   
					this->_borderheight = value ;
					for (int i = 0; i < (int)_itemsvector.size(); i++)
						_itemsvector[i]->BorderHeight = value ;
					this->_itemspanel->Height = value ;
				}
				float64 get(){ return 0.0 ;}
			}

			property float64  BorderWidth
			{
				void set(float64 value)
				{   
					this->_borderwidth = value ;
					for (int i = 0; i < (int)_itemsvector.size(); i++)
						_itemsvector[i]->BorderWidth = value ;
					this->_itemsgrid->Width = _borderwidth + _spacebetweenitems;
					this->Width = _borderwidth + _spacebetweenitems + 2 * _auxgridwidth ;
					this->_currentstackwidth = 2 * _auxgridwidth + this->_borderwidth + this->_spacebetweenitems ;
				}
				float64 get(){ return 0.0 ;}
			} 

		private:
			float64 _initialangle, _maxscale ; 
			Platform::String ^ _bordersource ;			 
			float64 _thumbheight, _thumbwidth, _borderheight, _borderwidth ;  
			   
#pragma endregion

#pragma region Properties

		public:
			property int32 StackNumber
			{
				void set(int32 value)
				{
					this->_stacknumber =  value ; 
				}
				int32 get(){ return this->_stacknumber ;}
			}

			property int32 NumberOfItems
			{
				void set(int32 value){ this->_numberofitems =  value ;}
				int32 get(){ return this->_numberofitems ;}
			}


			property float64 StackHeight
			{
				void set(float64 value)
				{ 
					this->_stackheight =  value ;
					this->Height =  value ;
					
				}
				float64 get(){ return this->_stackheight ;}
			}

			property float64 VerticalPosition
			{
				void set(float64 value)
				{ 
					this->_verticalposition = value ;
					this->_paneltransform->TranslateY =  value ;
					for (int i = 0; i < (int)_itemsvector.size(); i++)
						_itemsvector[i]->FullPositionY = -1 * value ;
				}
				float64 get(){ return this->_verticalposition ;}
			}

			property float64 MinStackWidth
			{
				void set(float64 value)
				{ 
					this->_minstackwidth =  value ;  
					this->_auxgridwidth = (value - _borderwidth) / 2 ;
					_begingrid->Width = this->_auxgridwidth ;
					_endgrid->Width = this->_auxgridwidth ;
					this->Width = _borderwidth + _spacebetweenitems + 2 * _auxgridwidth ;
					this->_currentstackwidth = 2 * _auxgridwidth + this->_borderwidth + this->_spacebetweenitems ;
				}
				float64 get(){ return this->_minstackwidth ;}
			}

			property float64 CurrentWidth
			{
				void set(float64 value)	{ }
				float64 get(){ return _currentstackwidth;}
			}
			
			property float64 SpaceBetweenItems
			{
				void set(float64 value)
				{   
					this->_spacebetweenitems =  value ;
					this->_itemsgrid->Width = _borderwidth + _spacebetweenitems;
					this->Width = _borderwidth + _spacebetweenitems + 2 * _auxgridwidth ;
					this->_currentstackwidth = 2 * _auxgridwidth + this->_borderwidth + this->_spacebetweenitems ;
				}
				float64 get(){ return this->_spacebetweenitems ; }
			}

			property bool IsOpen
			{
				void set(bool value){}
				bool get(){ return this->_isopen ;}
			}

			
			property DataSource::SectionDataSource^ Source
			{
				void set(DataSource::SectionDataSource^ value)
				{
					this->_datasource = value ; 
					initcomponent();
				}
				DataSource::SectionDataSource^ get(){return _datasource ;}
			}

			property float64 Position
			{
				void set(float64 value){ this->_position =  value ;}
				float64 get(){ return this->_position ;}
			} 

			property float64 DeviceWidth
			{
				void set(float64 value){ this->_devicewidth =  value ;}
				float64 get(){ return this->_devicewidth ;}
			} 

			property float64 DistanceToScreen
			{
				void set(float64 value){ 
					this->_distancetoscreen =  value ; 
					updatehorizontalposition();
				}
				float64 get(){ return this->_distancetoscreen ;}
			}

			property bool IsManipulating
			{
				void set(bool value)
				{
					_touches = 0 ;
					if(!value)
						for (int i = 0; i < (int)_itemsvector.size(); i++)
							_itemsvector[i]->IsManipulating  =  false ;

					Windows::UI::Xaml::Controls::Canvas::SetZIndex(this, 0);
				}
				bool get(){ return true ;}
			}

			 

			



		private:
			  

			bool _isopen ;
			int32 _stacknumber, _numberofitems ;
			float64 _minstackwidth , _currentstackwidth, _spacebetweenitems , _stackheight, _verticalposition;

			DataSource::SectionDataSource^ _datasource ;
			float64 _position , _distancetoscreen , _devicewidth ;

			///Auxiliar varibales
			int32 _touches , _selectedindex , _tempindex; //tempindex for stack size change
			float64 _angles[3] ;
			float64 _auxgridwidth , _tempposition , _tmpproportion ; //position to the tempindex from 0
			bool _itemslocked ;
#pragma endregion

#pragma region Controls
		public:
			property StackView::IStackItem ^ SelectedStackItem
			{
				void set(StackView::IStackItem ^ value){}
				StackView::IStackItem ^ get(){ return _selecteditem ;}
			}

			property IControls::SelectionType TypeSelected
			{
				void set(IControls::SelectionType value){}
				IControls::SelectionType get(){ return this->_selectiontype ; }
			}

			property float64 Proportion
			{
				void set(float64 value)
				{
					this->_proportion = value ; 
					if(value<1.0)
						_proportion = 1.0 ;
					if(value>_numberofitems)
						_proportion = _numberofitems ;					
					updateproportion();
				}	
				float64 get(){return this->_proportion ; }
			}

		private:
			StackView::IStackItem ^ _selecteditem ;
			IControls::SelectionType _selectiontype ;
			float64 _proportion ;

#pragma endregion

#pragma region Public Methods
		public:
			void LoadDataSource();
			void LoadFirst();
			void LoadBorder();
			void LoadSeparationItem() ;
			void StackManipulationCompleted();

			void SetToOpen();
			void SetToClose();

			float64 GetItemPosition(int32 page);
			void SetItemToFull(int32 page);
#pragma endregion

#pragma region Private Methods

		private:
			void initproperties();
			void initcomponent();
			void openstack();
			void closestack();

			void updateproportion();
			void updatehorizontalposition();

#pragma endregion

#pragma region Events Methods
		private:
			void StackItem_Selected(Platform::Object ^ sender , int32 _itemnumber);
			void StackItem_Tapped(Platform::Object ^ sender , int32 _itemnumber); 

			void ItemsPanel_PointerReleased_1(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e);
			void ItemsPanel_PointerPressed_1(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e);
			void ItemsPanel_Tapped_1(Platform::Object^ sender, Windows::UI::Xaml::Input::TappedRoutedEventArgs^ e);

			void StackItem_FullAnimationStarted(Platform::Object ^ sender , int32 chapter , int32 section , int32 page );
			void StackItem_FullAnimationCompleted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page  );
			void StackItem_ThumbAnimationStarted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page );
			void StackItem_ThumbAnimationCompleted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page );
			
#pragma endregion

#pragma region Animation

		private:
			Windows::UI::Xaml::Media::Animation::Storyboard^ _itemsgridstory;
			Windows::UI::Xaml::Media::Animation::DoubleAnimation^ _itemgridanimation ;

			Windows::UI::Xaml::Media::Animation::Storyboard^ _viewstory;
			Windows::UI::Xaml::Media::Animation::DoubleAnimation^ _viewanimation ;

			void initanimationproperties();
			void Storyboard_Completed_1(Platform::Object^ sender, Platform::Object^ e);
#pragma endregion

		};
	}
}