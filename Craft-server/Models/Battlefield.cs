using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Craft_server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Craft_server.Models
{
    public class Battlefield: ISubject
    {
        private List<IObserver> _observers;
        private readonly CoordinateContext _context;

        public Battlefield(CoordinateContext context)
        {
            _observers = new List<IObserver>();
            _context = context;
        }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Notify()
        {
            _observers.ForEach(o =>
            {
                 o.Update(this);
                //Add To Database
                _context.Entry(o).State = EntityState.Modified;
                _context.SaveChanges();
            });
        }
    }
}
