#if UN_AUTO_DEBUG
using System.Collections.Generic;

namespace RunTools
{
    public interface IAuto
    {
        List<GameCommand> CreateCommands();
    }
}
#endif
