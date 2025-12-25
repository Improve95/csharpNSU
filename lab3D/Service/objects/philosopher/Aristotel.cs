using IService.service;

namespace Service.objects.philosopher;

public class Aristotel(ITableManager tableManager, IStrategy strategy)
    : Philosopher("Аристотель", tableManager, strategy);