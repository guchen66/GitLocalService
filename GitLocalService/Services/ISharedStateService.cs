using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLocalService.Services
{
    public interface ISharedStateService
    {
        bool SelectedItem { get; set; }

        event Action<bool> ItemChanged;
    }

    public class SharedStateService : ISharedStateService
    {
        private bool _selectedItem;

        public bool SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                ItemChanged?.Invoke(value);
            }
        }

        public event Action<bool> ItemChanged;
    }
}