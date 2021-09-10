using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Infrastructure
{
    public class AppStateManager
    {
        public event Action<ComponentBase, string> StateChanged;

        public void UpdateCart(ComponentBase component)
        {
            StateChanged?.Invoke(component, "updatebasket");
        }

        public void LoginChanged(ComponentBase component)
        {
            StateChanged?.Invoke(component, "login");
        }

    }
}
