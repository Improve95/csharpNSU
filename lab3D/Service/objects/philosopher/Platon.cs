using IService.service;
using Service.service;
using Service.service.impl;

namespace Service.objects.philosopher;

public class Platon(ITableManager tableManager, Strategy strategy) 
    : Philosopher("Платон", tableManager, strategy) {}