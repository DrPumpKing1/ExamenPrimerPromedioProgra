using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal abstract class Entity: IClone<Entity>
    {
        public string name {  get; protected set; }
        public int lifeSpan {  get; protected set; }
        public int life { get; protected set; }

        public Entity(int lifeSpan, string name)
        {
            this.lifeSpan = lifeSpan;
            this.life = lifeSpan;
            this.name = name;
        }

        public virtual void Step()
        {
            life--;
            if(life == 0) Expire();
        }

        protected abstract void Expire();

        public abstract Entity Clone();
    }
}
