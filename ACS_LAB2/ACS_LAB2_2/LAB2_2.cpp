#include "Queue.h"
#include <future>
#include <thread>
#include <vector>

void writeQueue(ThreadSafeQueue* queue, size_t taskNum)
{
	uint8_t value = 1;
	for (size_t i = 0; i < taskNum; i++)
	{
		queue->push(value);
	}
}

void readQueue(ThreadSafeQueue *queue, int *p)
{
	int sum = 0;
	while (true)
	{
		uint8_t frontValue;
		if (!queue->pop(frontValue))
			break;
		else sum += frontValue;
	}

	*p = sum;

}


int main()
{

	std::vector<std::thread> c, p;
	std::vector<int> results(4, 0);
	ThreadSafeQueue queue;
	for (size_t i = 0; i < 4; i++)
	{

		c.push_back(std::thread(readQueue, &queue, &results.at(i)));
		p.push_back(std::thread(writeQueue, &queue, 10));

	}
	
	for (size_t i = 0; i < 4; i++)
	{
		p[i].join();
		c[i].join(); 

		//results.push_back(f[i].get());
	}
	int sum = 0;
	for (size_t i = 0; i < 4; i++)
	{
		//f[i].wait();
		
		sum += results[i];
		
	}
	//std::thread c1(readQueue, &queue, &p);
	//std::thread p1(writeQueue, &queue, 10);
	//c1.join();
	//p1.join();
	//int res =  f.get();
	//std::cout << res << std::flush;
	
	return 0;
}