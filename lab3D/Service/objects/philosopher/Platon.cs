using Service.service;
using Service.service.impl;

namespace Service.objects.philosopher;

public class Platon(IForkFactory forkFactory, Strategy strategy) 
    : Philosopher("Платон", forkFactory, strategy) {}