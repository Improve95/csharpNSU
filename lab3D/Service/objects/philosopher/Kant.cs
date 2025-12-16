using Service.service;
using Service.service.impl;

namespace Service.objects.philosopher;

public class Kant(IForkFactory forkFactory, Strategy strategy) 
    : Philosopher("Кант", forkFactory, strategy) {}