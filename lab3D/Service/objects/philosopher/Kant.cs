using IService.service;
using Service.service;

namespace Service.objects.philosopher;

public class Kant(ITableManager tableManager, Strategy strategy) 
    : Philosopher("Кант", tableManager, strategy) {}