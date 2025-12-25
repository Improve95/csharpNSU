using IService.service;

namespace Service.objects.philosopher;

public class Platon(ITableManager tableManager, IStrategy strategy) 
    : Philosopher("Платон", tableManager, strategy) {}