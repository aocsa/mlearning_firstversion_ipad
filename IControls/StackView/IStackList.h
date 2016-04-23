#include "pch.h"

namespace IControls
{
	namespace StackView
	{  		
		public delegate void StackListScrollToEventHandler(Platform::Object^ sender, float64 _position);
		public delegate void StackListAnimateToEventHandler(Platform::Object^ sender, float64 _position);
		public delegate void StackListWidthChangedEventHandler(Platform::Object^ sender, float64 _position);
		
		public ref class IStackList sealed : public Windows::UI::Xaml::Controls::Grid 
		{
		public:
			IStackList();

			event StackItemFullAnimationStartedEventHandler ^ StackItemFullAnimationStarted ;
			event StackItemFullAnimationCompletedEventHandler ^ StackItemFullAnimationCompleted ;
			event StackItemThumbAnimationStartedEventHandler ^ StackItemThumbAnimationStarted ;
			event StackItemThumbAnimationCompletedEventHandler ^ StackItemThumbAnimationCompleted ;

			event StackListScrollToEventHandler ^ StackListScrollTo ;
			event StackListAnimateToEventHandler ^ StackListAnimateTo ;
			event StackListWidthChangedEventHandler ^ StackListWidthChanged ;
			event IControlsComponentSelectedEventHandler ^ IControlsComponentSelected ;

#pragma region Paging Properties
		public:

			property int32 Chapter
			{
				void set(int32 value){ this->_chapter =  value ;}
				int32 get(){ return this->_chapter ;}
			} 

		private:
			int32 _chapter ;

#pragma endregion

#pragma region Controls

		private:
			Windows::UI::Xaml::Controls::StackPanel ^ _panel ;
			Windows::UI::Xaml::Controls::Grid ^ _auxgrid;
			Windows::UI::Xaml::Controls::Grid ^ _startgrid;
			//376
			Windows::UI::Xaml::Controls::Image ^ _startimage;
			///item replace image
			IControls::Components::ChapterHeaderControl ^ _headercontrol;
			float64 STARTWIDTH = 500;
			void initcontrols();
			std::vector<IStackView^> _stacksvector ;

#pragma endregion

#pragma region Stack  Properties

		public:  

			property Platform::String ^ BorderSource
			{
				void set(Platform::String ^ value){
					this->_bordersource = value ;
					for (int i = 0; i < (int)_stacksvector.size(); i++)
						_stacksvector[i]->BorderSource = value ;
				}
				Platform::String ^ get(){ return nullptr ; }
			} 

			property float64 MaxScale
			{
				void set(float64 value)	{ 
					this->_maxscale = value ; 
					for (int i = 0; i < (int)_stacksvector.size(); i++)
						_stacksvector[i]->MaxScale = value ;
				}
				float64 get(){ return 0.0 ;}
			}

			property float64 ThumbHeight
			{
				void set(float64 value)
				{  
					this->_thumbheight = value ; 
					for (int i = 0; i < (int)_stacksvector.size(); i++)
						_stacksvector[i]->ThumbHeight = value ;
				}
				float64 get(){ return 0.0 ;}
			}

			property float64  ThumbWidth
			{
				void set(float64 value)
				{
					this->_thumbwidth = value ; 
					for (int i = 0; i < (int)_stacksvector.size(); i++)
						_stacksvector[i]->ThumbWidth = value ;
				}
				float64 get(){ return 0.0 ;}
			}

			property float64  BorderHeight
			{
				void set(float64 value)
				{   
					this->_borderheight = value ;
					for (int i = 0; i < (int)_stacksvector.size(); i++)
						_stacksvector[i]->BorderHeight = value ;
				}
				float64 get(){ return 0.0 ;}
			}

			property float64  BorderWidth
			{
				void set(float64 value)
				{   
					this->_borderwidth = value ; 
					for (int i = 0; i < (int)_stacksvector.size(); i++)
						_stacksvector[i]->BorderWidth = value ;
				}
				float64 get(){ return 0.0 ;}
			} 

			property float64 StackVerticalPosition
			{
				void set(float64 value)
				{  
					this->_verticalposition = value ;
					for (int i = 0; i < (int)_stacksvector.size(); i++)
						_stacksvector[i]->VerticalPosition = value ;
				}
				float64 get(){ return this->_verticalposition ;}
			}

			property float64 MinStackWidth
			{
				void set(float64 value)
				{  
					this->_minstackwidth =  value ;
					for (int i = 0; i < (int)_stacksvector.size(); i++)
						_stacksvector[i]->MinStackWidth = value ;
				}
				float64 get(){ return this->_minstackwidth ;}
			}

			property float64 SpaceBetweenItems
			{
				void set(float64 value)
				{   
					this->_spacebetweenitems =  value ; 
					for (int i = 0; i < (int)_stacksvector.size(); i++)
						_stacksvector[i]->SpaceBetweenItems = value ;
				}
				float64 get(){ return this->_spacebetweenitems ; }
			}


			

	 

			

		private:
			  
			float64 _initialangle, _maxscale ; 
			Platform::String ^ _bordersource ;			 
			float64 _thumbheight, _thumbwidth, _borderheight, _borderwidth ;  			  
			float64 _verticalposition, _minstackwidth , _spacebetweenitems ;
#pragma endregion

#pragma region Properties
		public:

			property int32 ListNumber
			{
				void set(int32 value){ this->_listnumber = value ;}
				int32 get(){ return this->_listnumber ;}
			}

			property DataSource::ChapterDataSource^ Source
			{
				void set(DataSource::ChapterDataSource^ value)
				{
					this->_datasource =  value ; 
					loadcontrols();
				}
				DataSource::ChapterDataSource^ get(){return this->_datasource ;}
			}

			property float64 MinListWidth
			{
				void set(float64 value)
				{
					this->_minlistwidth =  value ;
					for (int i = 0; i < (int)_stacksvector.size(); i++)
						_stacksvector[i]->DeviceWidth = value ;
				}
				float64 get(){ return this->_minlistwidth ; }
			}

			property float64 ListHeight
			{
				void set(float64 value)
				{
					_listheight = value ;
					for (int i = 0; i < (int)_stacksvector.size(); i++)
						_stacksvector[i]->StackHeight = value ;
				}
				float64 get(){ return this->_listheight ; }
			}

			property float64 CurrentListWidth
			{
				void set(float64 value){ this->_currentlistwidth = value ;}
				float64 get(){ return this->_currentlistwidth ;}
			}


			property float64 Position
			{
				void set(float64 value){
					this->_position =  value ; 
				}
				float64 get(){ return this->_position ;}
			}

			property float64 DistanceToScreen
			{
				void set(float64 value){
					this->_distancetoscreen =  value ; 
					for(int i = 0 ; i<(int)_stacksvector.size(); i++)
						_stacksvector[i]->DistanceToScreen = _stacksvector[i]->Position + _distancetoscreen ;
				}
				float64 get(){ return this->_distancetoscreen ;}
			}

			property bool IsLoaded
			{
				void set(bool value){}
				bool get(){ return this->_isloaded ;}
			}

			property bool IsManipulating
			{
				void set(bool value){
					_ismanipulating =  value ;
					if(!value)
						for (int i = 0; i < (int)_stacksvector.size(); i++)
							_stacksvector[i]->IsManipulating = false ;
				}
				bool get(){ return this->_ismanipulating ;}
			}

			property IStackView ^ SelectedStack
			{
				void set(IStackView ^ value ){ }
				IStackView ^ get(){ return _selectedstack ;}
			}

			property IControls::SelectionType TypeSelected
			{
				void set(IControls::SelectionType value){}
				IControls::SelectionType get(){ return this->_selectiontype ; }
			}

		private:
			DataSource::ChapterDataSource^ _datasource ;
			float64 _minlistwidth , _listheight , _currentlistwidth ;
			float64 _position, _distancetoscreen ;
			bool _isloaded , _ismanipulating;
			int32 _listnumber , _selectedindex ;
			IStackView ^ _selectedstack ; 
			IControls::SelectionType _selectiontype ;
			//Auxiliar propeties
			int32 _numberofstacks ;
			float64 _tempdistoscreen ; 

#pragma endregion

#pragma region Public Methods
		public:
			void LoadDataSource();
			void UpDateProperties();
			void OpenStack(int32 index);
			float64 GetItemPosition(int32 section, int32 page);
			void SetItemToFull(int32 section, int32 page);
#pragma endregion

#pragma region Private Methods
		private:

			void loadcontrols();
			void initproperties();
			void updatewidth();
			
#pragma endregion

#pragma region Events Methods
		private:
			void Stack_SizeChangeStarted(Platform::Object^ sender, float64 pos );
			void Stack_SizeChangeDelta( Platform::Object^ sender, float64 pos );
			void Stack_SizeChangeCompleted(Platform::Object^ sender, float64 pos );
			void Stack_SizeAnimationStarted(Platform::Object ^sender ,  bool toopen);
			void Stack_SizeAnimationCompleted(Platform::Object ^sender ,  bool toopen );

			void IControls_ComponentSelected(Platform::Object ^ sender, SelectionType t , int32 index);

			void StackItem_FullAnimationStarted(Platform::Object ^ sender , int32 chapter , int32 section , int32 page );
			void StackItem_FullAnimationCompleted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page  );
			void StackItem_ThumbAnimationStarted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page );
			void StackItem_ThumbAnimationCompleted(Platform::Object ^ sender, int32 chapter , int32 section , int32 page );

#pragma endregion

		};
	}
}