using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using WPFHotelSystem.Models;

namespace WPFHotelSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("https://localhost:7260/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void SowPeople_Click(object sender, RoutedEventArgs e)
        {
            this.GetPeople();
        }
        private async void GetPeople()
        {
            var response = await client.GetStringAsync("People");
            var people = JsonConvert.DeserializeObject<List<People>>(response);
            dgPeople.ItemsSource = people;
        }
        private async void SavePeople(People people)
        {
            await client.PostAsJsonAsync("People/", people);
        }
        private async void UpdatePeople(People people)
        {
            await client.PutAsJsonAsync("People/" + people.PeopleId, people);
        }

        private async void DeletePeople(int PeopleId)
        {
            await client.DeleteAsync("People/" + PeopleId);
        }

        private void SavePeople_Click(object sender, RoutedEventArgs e)
        {
            var people = new People()
            {
                PeopleId = Convert.ToInt32(txtPeopleId.Text),
                PeopleName = txtPeopleName.Text,
                PeopleRoom = Convert.ToInt32(txtPeopleRoom.Text)
            };

            if (people.PeopleId == 0)
            {
                this.SavePeople(people);
            }
            else
            {
                this.UpdatePeople(people);
            }
            txtPeopleId.Text = 0.ToString();
            txtPeopleName.Text = "";
            txtPeopleRoom.Text = "";
        }

        private void dgPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void EditPeople(object sender, RoutedEventArgs e)
        {
            People people = ((FrameworkElement)sender).DataContext as People;
            txtPeopleId.Text = people.PeopleId.ToString();
            txtPeopleName.Text = people.PeopleName;
            txtPeopleRoom.Text = people.PeopleRoom.ToString();
        }

        void DeletePeople(object sender, RoutedEventArgs e)
        {
            People people = ((FrameworkElement)sender).DataContext as People;
            this.DeletePeople(people.PeopleId);
        }
    }
}
