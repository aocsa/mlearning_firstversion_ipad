#include "pch.h"

namespace IControls
{ 
	namespace StackView
	{ 

		public delegate void StackListScrollDeltaEventHandler(Platform::Object^ sender, float64 delta);
		public delegate void StackListScrollCompletedEventHandler(Platform::Object^ sender, int32 nextitem);

		public ref class IGroupList sealed : public Windows::UI::Xaml::Controls::Grid 
		{
		public:
			IGroupList();

			event StackItemFullAnimationStartedEventHandler ^ StackItemFullAnimationStarted ;
			event StackItemFullAnimationCompletedEventHandler ^ StackItemFullAnimationCompleted ;
			event StackItemThumbAnimationStartedEventHandler ^ StackItemThumbAnimationStarted ;
			event StackItemThumbAnimationCompletedEventHandler ^ StackItemThumbAnimationCompleted ;

			event StackListScrollDeltaEventHandler ^ StackListScrollDelta ;
			event StackListScrollCompletedEventHandler ^ StackListScrollCompleted;


#pragma region Controls

		private:
			Windows::UI::Xaml::Controls::Image ^_backimage;
			Windows::UI::Xaml::Controls::ScrollViewer^ _groupscroll;
			Windows::UI::Xaml::Controls::StackPanel^ _grouppanel ;
			Windows::UI::Xaml::Media::CompositeTransform^ _paneltransform ;
			void initcontrols();
			std::vector<IStackList ^> _listvector ;

			Windows::UI::Xaml::Controls::TextBlock ^ _texto ;

#pragma endregion

#pragma region Stack  Properties

		public:  

			property Platform::String ^ BorderSource
			{
				void set(Platform::String ^ value){
					this->_bordersource = value ;
					for (int i = 0; i < (int)_listvector.size(); i++)
						_listvector[i]->BorderSource = value ;
				}
				Platform::String ^ get(){ return nullptr ; }
			} 

			property float64 MaxScale
			{
				void set(float64 value)	{ 
					this->_maxscale = value ; 
					for (int i = 0; i < (int)_listvector.size(); i++)
						_listvector[i]->MaxScale = value ;
				}
				float64 get(){ return 0.0 ;}
			}

			property float64 ThumbHeight
			{
				void set(float64 value)
				{  
					this->_thumbheight = value ; 
					for (int i = 0; i < (int)_listvector.size(); i++)
						_listvector[i]->ThumbHeight = value ;
				}
				float64 get(){ return 0.0 ;}
			}

			property float64  ThumbWidth
			{
				void set(float64 value)
				{
					this->_thumbwidth = value ; 
					for (int i = 0; i < (int)_listvector.size(); i++)
						_listvector[i]->ThumbWidth = value ;
				}
				float64 get(){ return 0.0 ;}
			}

			property float64  BorderHeight
			{
				void set(float64 value)
				{   
					this->_borderheight = value ;
					for (int i = 0; i < (int)_listvector.size(); i++)
						_listvector[i]->BorderHeight = value ;
				}
				float64 get(){ return 0.0 ;}
			}

			property float64  BorderWidth
			{
				void set(float64 value)
				{   
					this->_borderwidth = value ; 
					for (int i = 0; i < (int)_listvector.size(); i++)
						_listvector[i]->BorderWidth = value ;
				}
				float64 get(){ return 0.0 ;}
			} 

			property float64 StackVerticalPosition
			{
				void set(float64 value)
				{  
					this->_verticalposition = value ;
					for (int i = 0; i < (int)_listvector.size(); i++)
						_listvector[i]->StackVerticalPosition = value ;
				}
				float64 get(){ return this->_verticalposition ;}
			}

			property float64 MinStackWidth
			{
				void set(float64 value)
				{  
					this->_minstackwidth =  value ;
					for (int i = 0; i < (int)_listvector.size(); i++)
						_listvector[i]->MinStackWidth = value ;
				}
				float64 get(){ return this->_minstackwidth ;}
			}

			property float64 SpaceBetweenItems
			{
				void set(float64 value)
				{   
					this->_spacebetweenitems =  value ; 
					for (int i = 0; i < (int)_listvector.size(); i++)
						_listvector[i]->SpaceBetweenItems = value ;
				}
				float64 get(){ return this->_spacebetweenitems ; }
			}

			 


		private: 

			float64  _maxscale ; 
			Platform::String ^ _bordersource ;			 
			float64 _thumbheight, _thumbwidth, _borderheight, _borderwidth ;  			  
			float64 _verticalposition, _minstackwidth , _spacebetweenitems ;

			//static properties
			float64 IThumbHeight = 150.0;
			float64 IThumbWidth = 267.0;
			float64 IFrameHeight = 305.0;
			float64 IFrameWidth = 210.0;
			float64 IDeviceWidth = 900.0;
			float64 IDeviceHeight = 1600.0;


#pragma endregion

#pragma region Properties
		public:
			property DataSource::BookDataSource^ Source
			{
				void set(DataSource::BookDataSource^ value)
				{
					this->_datasource = value ;
					loadcontrols();
				}
				DataSource::BookDataSource^ get(){return _datasource ;}
			}

			property float64 ControlWidth
			{
				void set(float64 value)
				{
					this->_controlwidth = value ;
					this->Width = value ;
					for (int i = 0; i < (int)_listvector.size(); i++)
						_listvector[i]->MinListWidth = value ;
				}
				float64 get(){ return this->_controlwidth ;}
			}

			property float64 ControlHeight
			{
				void set(float64 value)
				{
					this->_controlheight = value ;
					this->Height = value ;
					for (int i = 0; i < (int)_listvector.size(); i++)
						_listvector[i]->ListHeight = value ;
				}
				float64 get(){ return this->_controlheight ; }
			}

			property int32 StartIndex
			{
				int32 get(){ return _startindex;}
				void set(int32 value){ this->_startindex =  value  ;}
			}

			property IControls::StackView::IStackItem ^ SelectedStackItem
			{
				void set(IControls::StackView::IStackItem ^ value ){ }
				IControls::StackView::IStackItem ^ get(){ return _listvector[_selectedchapter]->SelectedStack->SelectedStackItem ;}
			}

			property int32 SelectedChapter
			{
				void set(int32 value){ this->_selectedchapter =  value ;}
				int32 get(){return this->_selectedchapter ;}
			}

			property int32 SelectedSection
			{
				void set(int32 value){ this->_selectedsection =  value ;}
				int32 get(){return this->_selectedsection ;}
			}

			property int32 SelectedPage
			{
				void set(int32 value){ this->_selectedpage =  value ;}
				int32 get(){return this->_selectedpage ;}
			}
					

		private:
			DataSource::BookDataSource^ _datasource ;
			float64 _controlheight , _controlwidth ;

			int32 _numberofitems , _startindex ;
			int32 _selectedchapter,  _selectedsection , _selectedpage ;
			///auxiliar variables
			int32 _touches ;
			bool _manipulationenable,  _forcemanipulationtoend ;
			bool _ismanipulating , _isinertia, _isitemselected;
			SelectionType _typeselected ;
			float64 _initthreshold , _finalthreshold ;
			float64 _paneltranslate , _offsetdelta ;
			//Constants for out manipulation
			float64 _leftconstant, _rigthconstant;
#pragma endregion


#pragma region Public Methods
		public:
			void LoadList(int32 stacknumber);
			void SetToItem(int32 chapter, int32 section, int32 page);
			void AnimateToChapter(int32 chapter);

			void TranslateTo(float64 value);
#pragma endregion

#pragma region Private Methods
		private:
			void initproperties();
			void loadcontrols();
			void computethresholds();
			void animatetoposition(float64 pos);
			void animatetochapter(int32 chapter, bool tobegin);
			void updatelistproperties(); 

#pragma endregion

#pragma region Manipulation Events Functions
		private :

			void chaptersVectorChanged_1(Windows::Foundation::Collections::IObservableVector<IControls::DataSource::ChapterDataSource ^> ^sender, Windows::Foundation::Collections::IVectorChangedEventArgs ^event);

			void Panel_PointerReleased_1(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e);
			void Panel_PointerPressed_1(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e);		
			void Panel_ManipulationDelta_1(Platform::Object^ sender, Windows::UI::Xaml::Input::ManipulationDeltaRoutedEventArgs^ e);
			void Panel_ManipulationCompleted_1(Platform::Object^ sender, Windows::UI::Xaml::Input::ManipulationCompletedRoutedEventArgs^ e);
			void Panel_ManipulationInertiaStarting_1(Platform::Object^ sender, Windows::UI::Xaml::Input::ManipulationInertiaStartingRoutedEventArgs^ e);	

#pragma endregion

#pragma region Stack Events Functions
		private:
			void StackItem_FullAnimationStarted(Platform::Object ^ sender , int32 chapter , int32 section , int32 page );
			void StackItem_FullAnimationCompleted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page );
			void StackItem_ThumbAnimationStarted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page );
			void StackItem_ThumbAnimationCompleted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page );

			void StackList_ScrollTo(Platform::Object^ sender, float64 _position);
			void StackList_AnimateTo(Platform::Object^ sender, float64 _position);
			void StackList_WidthChanged(Platform::Object^ sender, float64 _position);
			void IControls_ComponentSelected(Platform::Object ^ sender, SelectionType t , int32 index);

#pragma endregion

			
#pragma region Animation 

		private: 
			Windows::UI::Xaml::Media::Animation::Storyboard^ _panelstory;
			Windows::UI::Xaml::Media::Animation::DoubleAnimation^ _panelanimation ;
			void initanimationproperties();			
			void Storyboard_Completed_1(Platform::Object^ sender, Platform::Object^ e);

#pragma endregion
		};
	}
}