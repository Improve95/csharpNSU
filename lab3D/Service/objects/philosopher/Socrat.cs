using IService.service;
using Service.service;

namespace Service.objects.philosopher;

public class Socrat(ITableManager tableManager, Strategy strategy) 
    : Philosopher("Сократ", tableManager, strategy) {}