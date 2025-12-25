using IService.service;

namespace Service.objects.philosopher;

public class Kant(ITableManager tableManager, IStrategy strategy) 
    : Philosopher("Кант", tableManager, strategy) {}