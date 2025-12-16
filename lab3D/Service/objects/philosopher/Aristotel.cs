using IService.service;
using Service.service;

namespace Service.objects.philosopher;

public class Aristotel(ITableManager tableManager, Strategy strategy)
    : Philosopher("Аристотель", tableManager, strategy);