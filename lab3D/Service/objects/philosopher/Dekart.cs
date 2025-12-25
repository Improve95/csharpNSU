using IService.service;

namespace Service.objects.philosopher;

public class Dekart(ITableManager tableManager, IStrategy strategy) 
    : Philosopher("Декарт",  tableManager, strategy) {}