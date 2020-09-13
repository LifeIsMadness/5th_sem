#include "Queue.h"
#include <future>
#include <thread>

void readQueue(ThreadSafeQueue *queue, std::promise<int> *p)
{
	int sum = 0;
	while (true)
	{
		uint8_t frontValue;
		if (!queue->pop(frontValue))
			break;
		else sum += frontValue;
	}

	p->set_value(sum);

}


int main()
{
	std::promise<int> p;
	std::future<int> f = p.get_future();
	ThreadSafeQueue queue;
	std::thread t1(readQueue, &queue, &p);
	t1.join();
	int res =  f.get();
	std::cout << res << std::flush;
	
	return 0;
}