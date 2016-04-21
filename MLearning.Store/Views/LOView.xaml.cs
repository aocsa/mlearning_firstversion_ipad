using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Cirrious.MvvmCross.WindowsCommon.Views;
using StackView;
using DataSource;
using MLearning.Core.ViewModels;
using Windows.UI.Xaml.Media.Imaging;
using MLearning.Store.Components;
using Windows.UI;
using System.Collections.ObjectModel;
using MLReader;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MLearning.Store.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LOView : MvxWindowsPage
    {
        BookDataSource booksource;
        IGroupList lo_list;
        ControlDownMenu menu;
        int _currentLO = 0;

        public LOView()
        {
            this.InitializeComponent();
            this.Loaded += LOView_Loaded;
        }

        void LOView_Loaded(object sender, RoutedEventArgs e)
        {
            initbackground();
            init();
            initreader();
        }



        void init()
        {
            booksource = new BookDataSource();
            lo_list = new IGroupList();

            lo_list.StackListScrollCompleted += lo_list_StackListScrollCompleted;
            lo_list.StackItemFullAnimationCompleted += lo_list_StackItemFullAnimationCompleted;
            lo_list.StackItemFullAnimationStarted += lo_list_StackItemFullAnimationStarted;
            MainGrid.Children.Add(lo_list);

            menu = new ControlDownMenu();
            MainGrid.Children.Add(menu);
            menu.ControlDownElementSelected += menu_ControlDownElementSelected;

            loadLOsInCircle(0);
            lo_list.Source = booksource;
            menu.Source = booksource;
            var vm = this.ViewModel as LOViewModel;
            vm.PropertyChanged += vm_PropertyChanged;

        }

        void lo_list_StackItemFullAnimationStarted(object sender, int chapter, int section, int page)
        {
            LoadPagesDataSource();

        }

        void lo_list_StackItemFullAnimationCompleted(object sender, int chapter, int section, int page)
        {

            Canvas.SetZIndex(_readerview, 1000);
        }

        private void lo_list_StackListScrollCompleted(object sender, int nextitem)
        {
            menu.SelectElement(nextitem);
            booksource.TemporalColor = booksource.Chapters[nextitem].ChapterColor;
            _backgroundscroll.settoindex(nextitem);
            _menucontroller.Animate2Color(booksource.Chapters[nextitem].ChapterColor);
        }

        void menu_ControlDownElementSelected(object sender, int index)
        {
            lo_list.AnimateToChapter(index);
            var vm = ViewModel as LOViewModel;
            // vm.LoadStackImagesCommand.Execute(index);
        }

        void vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var vm = this.ViewModel as LOViewModel;
            if (e.PropertyName == "LOsInCircle")
            {
                if (vm.LOsInCircle != null)
                {
                    vm.LOsInCircle.CollectionChanged += LOsInCircle_CollectionChanged;
                }
            }

        }



        void LOsInCircle_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            loadLOsInCircle(e.NewStartingIndex);
            var vm = ViewModel as LOViewModel;
            menu.SelectElement(vm.LOCurrentIndex);
            booksource.TemporalColor = Util.GetColorbyIndex(vm.LOCurrentIndex);
        }

        void loadLOsInCircle(int index)
        {
            var vm = ViewModel as LOViewModel;
            if (vm.LOsInCircle != null)
            {

                for (int i = index; i < vm.LOsInCircle.Count; i++)
                {
                    ChapterDataSource newchapter = new ChapterDataSource();
                    newchapter.Title = vm.LOsInCircle[i].lo.title;
                    newchapter.Author = vm.LOsInCircle[i].lo.name + "\n" + vm.LOsInCircle[i].lo.lastname;
                    newchapter.Description = vm.LOsInCircle[i].lo.description;
                    newchapter.ChapterColor = Util.GetColorbyIndex(i % 6);
                    newchapter.TemporalColor = Util.GetColorbyIndex(0);
                    // newchapter.BackgroundImage =
                    if (vm.LOsInCircle[i].background_bytes != null)
                        newchapter.BackgroundImage = Constants.ByteArrayToImageConverter.Convert(vm.LOsInCircle[i].background_bytes);

                    vm.LOsInCircle[i].PropertyChanged += (s1, e1) =>
                            {
                                if (e1.PropertyName == "background_bytes")
                                {
                                    newchapter.BackgroundImage = Constants.ByteArrayToImageConverter.Convert(vm.LOsInCircle[i].background_bytes);
                                }
                            };

                    //loading the stacks
                    if (vm.LOsInCircle[i].stack.IsLoaded)
                    {
                        var s_list = vm.LOsInCircle[i].stack.StacksList;
                        for (int j = 0; j < s_list.Count; j++)
                        {
                            SectionDataSource stack = new SectionDataSource();

                            stack.Name = s_list[j].TagName;
                            for (int k = 0; k < s_list[j].PagesList.Count; k++)
                            {
                                var page = new PageDataSource();
                                page.Name = s_list[j].PagesList[k].page.title;
                                page.Description = s_list[j].PagesList[k].page.description;
                                if (s_list[j].PagesList[k].cover_bytes != null)
                                    page.ImageContent = Constants.ByteArrayToImageConverter.Convert(s_list[j].PagesList[k].cover_bytes);
                                s_list[j].PagesList[k].PropertyChanged += (s2, e2) =>
                                {
                                    if (e2.PropertyName == "cover_bytes")
                                        page.ImageContent = Constants.ByteArrayToImageConverter.Convert((s2 as MLearning.Core.ViewModels.LOViewModel.page_wrapper).cover_bytes);//s_list[j].PagesList[k].cover_bytes);
                                };
                                stack.Pages.Add(page);
                            }
                            newchapter.Sections.Add(stack);
                        }

                    }
                    else
                    {

                        vm.LOsInCircle[i].stack.PropertyChanged += (s3, e3) =>
                            {
                                var s_list = vm.LOsInCircle[i].stack.StacksList;
                                for (int j = 0; j < s_list.Count; j++)
                                {
                                    SectionDataSource stack = new SectionDataSource();

                                    stack.Name = s_list[j].TagName;
                                    for (int k = 0; k < s_list[j].PagesList.Count; k++)
                                    {
                                        PageDataSource page = new PageDataSource();
                                        page.Name = s_list[j].PagesList[k].page.title;
                                        page.Description = s_list[j].PagesList[k].page.description;
                                        if (s_list[j].PagesList[k].cover_bytes != null)
                                            page.ImageContent = Constants.ByteArrayToImageConverter.Convert(s_list[j].PagesList[k].cover_bytes);
                                        s_list[j].PagesList[k].PropertyChanged += (s2, e2) =>
                                        {
                                            if (e2.PropertyName == "cover_bytes")
                                                page.ImageContent = Constants.ByteArrayToImageConverter.Convert(s_list[j].PagesList[k].cover_bytes);
                                        };
                                        stack.Pages.Add(page);
                                    }
                                    newchapter.Sections.Add(stack);
                                }
                            };
                    }
                    booksource.Chapters.Add(newchapter);
                }
                menu.SelectElement(vm.LOCurrentIndex);
                booksource.TemporalColor = Util.GetColorbyIndex(vm.LOCurrentIndex);
                _backgroundscroll.Source = booksource;
                _menucontroller.SEtColor(booksource.Chapters[vm.LOCurrentIndex].ChapterColor);
            }
        }

        #region Controls background

        ControlScrollView _backgroundscroll;
        UpMenuController _menucontroller;
        void initbackground()
        {
            _backgroundscroll = new ControlScrollView();
            MainGrid.Children.Add(_backgroundscroll);
            Canvas.SetZIndex(_backgroundscroll, -10);

            _menucontroller = new UpMenuController();
            MainGrid.Children.Add(_menucontroller);
            Canvas.SetZIndex(_menucontroller, 100);
        }

        #endregion


        #region Reader Datasource Load

        List<LOPageSource> pagelistsource = new List<LOPageSource>();

        void LoadPagesDataSource()
        {
            var vm = ViewModel as LOViewModel;
            var styles = new StyleConstants();
            for (int i = 0; i < vm.LOsInCircle.Count; i++)
            {
                var s_list = vm.LOsInCircle[i].stack.StacksList;
                for (int j = 0; j < s_list.Count; j++)
                {

                    for (int k = 0; k < s_list[j].PagesList.Count; k++)
                    {
                        LOPageSource page = new LOPageSource();
                        var content = s_list[j].PagesList[k].content;

                        page.Cover = Constants.ByteArrayToImageConverter.Convert(s_list[j].PagesList[k].cover_bytes);
                        page.PageIndex = k;
                        page.StackIndex = j;
                        page.LOIndex = i;
                        var slides = s_list[j].PagesList[k].content.lopage.loslide;
                        page.Slides = new List<LOSlideSource>();
                        for (int m = 0; m < slides.Count; m++)
                        {
                            LOSlideSource slidesource = new LOSlideSource();
                            slidesource.Style = styles.stylesList[i][slides[m].lotype];
                            slidesource.Type = slides[m].lotype;
                            if (slides[m].lotitle != null) slidesource.Title = slides[m].lotitle;
                            if (slides[m].loparagraph != null) slidesource.Paragraph = slides[m].loparagraph;
                            if (slides[m].loimage != null) slidesource.ImageUrl = slides[m].loimage;
                            if (slides[m].lotext != null) slidesource.Paragraph = slides[m].lotext;
                            if (slides[m].loauthor != null) slidesource.Author = slides[m].loauthor;
                            if (slides[m].lovideo != null) slidesource.VideoUrl = slides[m].lovideo;

                            if (slides[m].loitemize != null)
                            {
                                slidesource.Itemize = new ObservableCollection<LOItemSource>();
                                var items = slides[m].loitemize.loitem;
                                for (int n = 0; n < items.Count; n++)
                                {
                                    LOItemSource item = new LOItemSource();
                                    if (items[n].loimage != null) item.ImageUrl = items[n].loimage;
                                    if (items[n].lotext != null) item.Text = items[n].lotext;
                                    slidesource.Itemize.Add(item);
                                }
                            }
                            page.Slides.Add(slidesource);
                        } 
                        //pages

                        pagelistsource.Add(page);
                    }
                }
            }

            //add pages
            _readerview.Source = pagelistsource;
            //Canvas.SetZIndex(_readerview, 10);
        }


        #endregion


        #region Reader View

        LOReaderScroll _readerview;

        void initreader()
        {
            _readerview = new LOReaderScroll();
            //_readerview.LOReaderLoadPageAt += _readerview_LOReaderLoadPageAt;
            MainGrid.Children.Add(_readerview);
            Canvas.SetZIndex(_readerview, -100);
        }

        void _readerview_LOReaderLoadPageAt(object sender, int lo, int stack, int page)
        {
            var vm = ViewModel as LOViewModel;
            vm.OpenPageCommand.Execute(vm.LOsInCircle[lo].stack.StacksList[stack].PagesList[page]);
        }

        #endregion

    }
}
