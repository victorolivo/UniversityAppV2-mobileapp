using System;
using Xamarin.Forms;
using UniversityAppV2.Services;
using UniversityAppV2.Models;
using System.Threading.Tasks;

namespace UniversityAppV2
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();

            //Load initial terms list
            termList.ItemsSource = TermsDB.GetAllTerms();
        }

        //Trigger: Event (User wants to add a new term)
        //Action: Takes user to a new page to enter and save new term details
        private void AddTermBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddModTerm(false, null));
        }

        //Trigger: Event (User taps/selects a term)
        //Action: Deselect the term; 'termList_ItemSelected' event also triggered
        private void termList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        //Trigger: User refreshes page (Pulls down page)
        //Action: Updates the terms list
        public async void TermsRefreshView_Refreshing(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            termList.ItemsSource = TermsDB.GetAllTerms();
            TermsRefreshView.IsRefreshing = false;
        }

        //Trigger: Automatically called when user opens page
        //Action: Updates the terms list; Keeps list up-to-date
        protected override void OnAppearing()
        {
            termList.ItemsSource = TermsDB.GetAllTerms();
            base.OnAppearing();
        }

        //Trigger: Event (User wants to modify a term)
        //Action: Takes user to a new page to enter and save term details
        private void ModifyTerm_Clicked(object sender, EventArgs e)
        {
            var term = ((MenuItem)sender).BindingContext as Term;
            Navigation.PushAsync(new AddModTerm(true, term));

        }

        //Trigger: Event (User wants to delete a term)
        //Action: Deletes term from the database; Refreshes view
        private void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            var term = ((MenuItem)sender).BindingContext as Term;

            TermsDB.RemoveTerm(term.Id);

            TermsRefreshView_Refreshing(sender, e);
        }

        //Trigger: Event (User taps/selects a term)
        //Action: Takes user inside the selected term (Term details/Courses Page)
        private async void termList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var term = ((ListView)sender).SelectedItem as Term;
            if (term == null)
                return;
            ((ListView)sender).SelectedItem = null;
            await Navigation.PushAsync(new CoursesPage(term));
        }
    }
}
