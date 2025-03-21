using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LinearListApp.Models;
using ReactiveUI;
using System.Reactive.Linq;

namespace LinearListApp.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly LinearList<string> _linearList;
        private string _newItem;

        public MainWindowViewModel()
        {
            _linearList = new LinearList<string>();
            _newItem = string.Empty;

            var addCommand = ReactiveCommand.Create(AddItem);
            var removeCommand = ReactiveCommand.Create(RemoveItem);
            var moveNextCommand = ReactiveCommand.Create(MoveNext);
            var moveToStartCommand = ReactiveCommand.Create(MoveToStart);

            AddCommand = addCommand;
            RemoveCommand = removeCommand;
            MoveNextCommand = moveNextCommand;
            MoveToStartCommand = moveToStartCommand;

            addCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine($"AddCommand error: {ex.Message}"));
            removeCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine($"RemoveCommand error: {ex.Message}"));
            moveNextCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine($"MoveNextCommand error: {ex.Message}"));
            moveToStartCommand.ThrownExceptions.Subscribe(ex => Console.WriteLine($"MoveToStartCommand error: {ex.Message}"));
        }

        public string CurrentElement => _linearList.CurrentElement ?? "Нет текущего элемента";
        public int Count => _linearList.Count;
        public bool IsEmpty => _linearList.IsEmpty;

        // Обновлённое свойство: "Пусто", если список пуст
        public string ItemsString => _linearList.Items.Count > 0 ? string.Join(", ", _linearList.Items) : "Пусто";

        public string NewItem
        {
            get => _newItem;
            set => this.RaiseAndSetIfChanged(ref _newItem, value);
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand MoveNextCommand { get; }
        public ICommand MoveToStartCommand { get; }

        private void AddItem()
        {
            try
            {
                Console.WriteLine("AddItem called");
                if (!string.IsNullOrWhiteSpace(NewItem))
                {
                    _linearList.Add(NewItem);
                    NewItem = string.Empty;
                    this.RaisePropertyChanged(nameof(CurrentElement));
                    this.RaisePropertyChanged(nameof(Count));
                    this.RaisePropertyChanged(nameof(IsEmpty));
                    this.RaisePropertyChanged(nameof(ItemsString));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddItem: {ex.Message}");
                throw;
            }
        }

        private void RemoveItem()
        {
            try
            {
                Console.WriteLine("RemoveItem called");
                if (_linearList.CurrentElement != null)
                {
                    _linearList.Remove(_linearList.CurrentElement);
                    this.RaisePropertyChanged(nameof(CurrentElement));
                    this.RaisePropertyChanged(nameof(Count));
                    this.RaisePropertyChanged(nameof(IsEmpty));
                    this.RaisePropertyChanged(nameof(ItemsString));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RemoveItem: {ex.Message}");
                throw;
            }
        }

        private void MoveNext()
        {
            try
            {
                Console.WriteLine("MoveNext called");
                _linearList.MoveNext();
                this.RaisePropertyChanged(nameof(CurrentElement));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MoveNext: {ex.Message}");
                throw;
            }
        }

        private void MoveToStart()
        {
            try
            {
                Console.WriteLine("MoveToStart called");
                _linearList.MoveToStart();
                this.RaisePropertyChanged(nameof(CurrentElement));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MoveToStart: {ex.Message}");
                throw;
            }
        }
    }
}