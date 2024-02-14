using Microsoft.AspNetCore.Components;
using DataTemplateLibrary.Models;
using ServerManagement;
using LoginService;
using CookieService;
using System;


namespace Taskel.Pages
{
    
    public partial class ServicePage : ComponentBase
    {
        [Inject] public ServerManager? ServerManager { get; set; }
        [Parameter] public int ServiceID { get; set; }

        private DBService? Service { get; set; }

        protected override void OnInitialized()
        {
            if(ServerManager != null)
            {
                Service = ServerManager.GetService(ServiceID);
            }         
        }

        protected string ServiceName
        {
            get
            {
                return Service != null ? Service.ServiceName : ""; ;
            }
        }
        protected string ServiceShortDescription
        {
            get
            {
                return Service != null ? Service.ShortDescription : "";
            }
        }
        protected string ServiceLongDescription
        {
            get
            {
                return Service != null ? Service.LongDescription : "";
            }
        }
        protected DateOnly ServiceCreation
        {
            get
            {
                return Service != null ? Service.Created : DateOnly.MinValue;
            }
        }
        protected int ServicePrice
        {
            get
            {
                return Service != null ? Service.CurrentPrice : 0;
            }
        }
        protected int ServiceOwner
        {
            get
            {
                return Service != null ? Service.UserId : 0;
            }
        }

    }
}
