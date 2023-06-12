using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IUpdatable
{
    Layer UpdateLayer { get; }
    void Tick();
    void LateTick();
}
