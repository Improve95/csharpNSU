using IService.service;
using Service.service;
using Service.service.impl;

namespace Service.objects.philosopher;

public class Kant(ITableManager tableManager, Strategy strategy) 
    : Philosopher("Кант", tableManager, strategy) {}