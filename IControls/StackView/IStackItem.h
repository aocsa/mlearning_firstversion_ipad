#include "pch.h"

namespace IControls
{
	namespace StackView
	{ 
		public delegate void StackItemSelectedEventHandler(Platform::Object ^ sender , int32 _itemnumber);
		public delegate void StackItemTappedEventHandler(Platform::Object ^ sender , int32 _itemnumber);

		public ref class IStackItem sealed : public Windows::UI::Xaml::Controls::Grid 
		{
		public:
			IStackItem();

#pragma region Controls 

		private:  
			Windows::UI::Xaml::Controls::Image ^ _thumbimage ;
			Windows::UI::Xaml::Controls::Image ^ _borderimage;
			Windows::UI::Xaml::Controls::Border ^ _bordercolor;


			Windows::UI::Xaml::Controls::Grid ^ _itemgrid ;
			Windows::UI::Xaml::Controls::Image ^ _itemimage ;

			Windows::UI::Xaml::Media::CompositeTransform ^  _transform ; 
			void initcontrols();


			//Controls for text
			Windows::UI::Xaml::Controls::StackPanel ^ _textpanel;
			Windows::UI::Xaml::Controls::TextBlock^ _titletext;
			Windows::UI::Xaml::Controls::TextBlock ^ _descriptiontext;
			Windows::UI::Xaml::Media::CompositeTransform ^  _texttransform;

#pragma endregion

#pragma region Events

		public:
			event StackItemSelectedEventHandler ^ StackItemSelected ;
			event StackItemTappedEventHandler^ StackItemTapped ;
			
			event StackItemFullAnimationStartedEventHandler ^ StackItemFullAnimationStarted ;
			event StackItemFullAnimationCompletedEventHandler ^ StackItemFullAnimationCompleted ;
			event StackItemThumbAnimationStartedEventHandler ^ StackItemThumbAnimationStarted ;
			event StackItemThumbAnimationCompletedEventHandler ^ StackItemThumbAnimationCompleted ;
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

			property int32 Page
			{
				void set(int32 value){ this->_page = value ;}
				int32 get(){ return this->_page  ;}
			}

		private:
			int32 _chapter, _section , _page ;
#pragma endregion


#pragma region Properties

		public:

			property int32 ItemNumber
			{
				void set(int32 value)
				{ 
					this->_itemnumber =  value ; 
					ZIndex = _maxthreshold - _itemnumber ; 
				}
				int32 get(){ return this->_itemnumber ;}
			}

			property float64 InitialAngle
			{
				void set(float64 value)
				{ 
					this->_initialangle =  value ;  
					this->_transform->Rotation = value ; 
				}
				float64 get(){ return this->_initialangle ;}
			}

			property int32 ZIndex
			{
				void set(int32 value){ Windows::UI::Xaml::Controls::Canvas::SetZIndex(this , value ); }
				int32 get(){ return Windows::UI::Xaml::Controls::Canvas::GetZIndex(this) ; }
			}

			property DataSource::PageDataSource ^ Source
			{
				void set(DataSource::PageDataSource ^ value)
				{
					this->_datasource = value ;
				}
				DataSource::PageDataSource ^ get(){ return _datasource;}
			}

			property Platform::String ^ BorderSource
			{
				void set(Platform::String ^ value){ this->_bordersource = value ; }
				Platform::String ^ get(){ return _bordersource ; }
			}

			property Windows::UI::Xaml::Media::CompositeTransform ^ ItemTransform
			{
				void set(Windows::UI::Xaml::Media::CompositeTransform ^ value){}
				Windows::UI::Xaml::Media::CompositeTransform^ get(){return this->_transform;} 
			}

			property float64 MaxScale
			{
				void set(float64 value)	{  this->_maxscale =  value ; }
				float64 get(){ return this->_maxscale ;}
			}

			property float64 ThumbHeight
			{
				void set(float64 value)
				{  
					this->_thumbheight =  value ;
					this->_thumbimage->Height = value ;
					_bordercolor->Height = value +12;
					this->_itemgrid->Height = value ;
				}
				float64 get(){ return this->_thumbheight ;}
			}

			property float64 ThumbWidth
			{
				void set(float64 value)
				{  
					this->_thumbwidth = value ;
					this->_thumbimage->Width = value ;
					_bordercolor->Width = value +12;
					this->_itemgrid->Width =  value ;
				}
				float64 get(){ return this->_thumbwidth ;}
			}

			property float64 BorderHeight
			{
				void set(float64 value)
				{  
					this->_borderheight =  value ;
					this->Height = value ;
					//this->_borderimage->Height = value;
					
					this->_transform->CenterY = value / 2 ;
				}
				float64 get(){ return this->_thumbheight ;}
			}

			property float64 BorderWidth
			{
				void set(float64 value)
				{  
					this->_borderwidth = value ;
					this->Width =  value ;
					//this->_borderimage->Width =  value  ;
					
					this->_transform->CenterX = value / 2 ;
				}
				float64 get(){ return this->_thumbwidth ;}
			}


			property float64 InitialPosition
			{
				void set(float64 value)
				{  
					this->_initialposition =  value ;
					if(!_isopen)
						_transform->TranslateX =  value ;
				}
				float64 get(){ return this->_initialposition ;}
			}

			property float64 FinalPosition
			{
				void set(float64 value)
				{  
					this->_finalposition = value ;
					if(_isopen && !_isfull)
						_transform->TranslateX = value ;
				}
				float64 get(){ return this->_finalposition ;}
			}

			property float64 FullPositionX
			{
				void set(float64 value)	{  
					this->_fullpositionx = value ; 
				}
				float64 get(){ return this->_fullpositionx ;}
			}

			property float64 FullPositionY
			{
				void set(float64 value)	{  
					this->_fullpositiony = value ; 
				}
				float64 get(){ return this->_fullpositiony ;}
			}
			
			property bool IsOpen
			{
				void set(bool value){ this->_isopen =  value ;}
				bool get(){ return this->_isopen ;}
			}

			property bool IsFull
			{
				void set(bool value){ this->_isfull = value ; }
				bool get(){ return this->_isfull ; }
			}

			property bool IsManipulating
			{
				void set(bool value){ _touches = 0 ; }
				bool get(){ return true ; }
			}
			 

		private: 
			 
			//Color of the border brush
			Windows::UI::Xaml::Media::SolidColorBrush^ _borderbrushcolor;

			int32 _itemnumber ;
			float64 _initialangle, _maxscale ;
			DataSource::PageDataSource ^ _datasource ;
			Platform::String ^ _bordersource ;
			 
			float64 _thumbheight, _thumbwidth, _borderheight, _borderwidth ;
			float64 _initialposition, _finalposition ;
			float64 _fullpositionx , _fullpositiony ;

			bool _isopen, _isfull, _isselected ;
			
			//auxiliar variables
			int32 _touches, _maxthreshold ;
			bool _ismfull ;

#pragma endregion

#pragma region Public Methods
		public:
			void LoadThumbSource();
			void LoadBorder();
			void LoadFullSource();
			void DeleteFullSource();
			void ItemManipulationCompleted();

			void AnimateToOpen();
			void AnimateToClose();
			void SetToOpen();
			void SetToClose();
			void AnimateToFull();
			void SetToFull();
			void AnimateToThumb();
			void SetToThumb();

			void ShowText();
			void HiddeText();

#pragma endregion

#pragma region Private Methods
		private:
			void initproperties(); 
			void animatecolor();
			void animate2double(float64 to);
			void animate2opacity(float64 to);

#pragma endregion

#pragma region Events Methods

		private:
			void StackItem_PointerPressed(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e);
			void StackItem_PointerReleased(Platform::Object^ sender, Windows::UI::Xaml::Input::PointerRoutedEventArgs^ e);
			void StackItem_Tapped(Platform::Object^ sender, Windows::UI::Xaml::Input::TappedRoutedEventArgs^ e);
			void StackItem_StoryboardCompleted(Platform::Object^ sender, Platform::Object^ e);
			void StackItem_StoryboardFullCompleted(Platform::Object^ sender, Platform::Object^ e);

			void OnPropertyChanged(Platform::Object ^sender, Windows::UI::Xaml::Data::PropertyChangedEventArgs ^e);


			void datasourcePropertyChanged(Platform::Object ^sender, Windows::UI::Xaml::Data::PropertyChangedEventArgs ^e);

#pragma endregion

#pragma region Animations

		private :  
			//open close
			Windows::UI::Xaml::Media::Animation::Storyboard ^ _translatestory ;
			Windows::UI::Xaml::Media::Animation::DoubleAnimation ^ _translateXanimation;
			Windows::UI::Xaml::Media::Animation::Storyboard ^ _rotatestory ;
			Windows::UI::Xaml::Media::Animation::DoubleAnimation ^ _rotateanimation;

			//to full
			Windows::UI::Xaml::Media::Animation::Storyboard^ _translatestory1 ;
			Windows::UI::Xaml::Media::Animation::DoubleAnimation^ _translatexanimation1 ;
			Windows::UI::Xaml::Media::Animation::DoubleAnimation^ _translateyanimation1 ;

			Windows::UI::Xaml::Media::Animation::Storyboard^ _scalestory1 ;
			Windows::UI::Xaml::Media::Animation::DoubleAnimation^ _scalexanimation1 ;
			Windows::UI::Xaml::Media::Animation::DoubleAnimation^ _scaleyanimation1 ;

			Windows::UI::Xaml::Media::Animation::Storyboard^ _rotatestory1;
			Windows::UI::Xaml::Media::Animation::DoubleAnimation^ _rotateanimation1 ;
			void inititemanimations();			

#pragma endregion

		};
	}
}