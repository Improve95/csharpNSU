using IService.service;
using Service.service;
using Service.service.impl;

namespace Service.objects.philosopher;

public class Dekart(ITableManager tableManager, Strategy strategy) 
    : Philosopher("Декарт",  tableManager, strategy) {}