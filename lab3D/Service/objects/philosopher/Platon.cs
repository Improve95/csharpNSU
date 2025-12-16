using IService.service;
using Service.service;

namespace Service.objects.philosopher;

public class Platon(ITableManager tableManager, Strategy strategy) 
    : Philosopher("Платон", tableManager, strategy) {}