using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawableWrapperRegistryImplementation
{
  public interface IDrawableWrapperFactory<TDrawableEntity>
  {
    IDrawableWrapper ()
  }
}
