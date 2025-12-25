using IService.service;

namespace Service.objects.philosopher;

public class Socrat(ITableManager tableManager, IStrategy strategy) 
    : Philosopher("Сократ", tableManager, strategy) {}