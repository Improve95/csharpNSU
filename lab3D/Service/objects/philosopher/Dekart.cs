using IService.service;
using Service.service;

namespace Service.objects.philosopher;

public class Dekart(ITableManager tableManager, Strategy strategy) 
    : Philosopher("Декарт",  tableManager, strategy) {}