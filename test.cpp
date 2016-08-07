#include <iostream>
#include <stdlib.h>
#include <string>
using namespace std;



class ListNode
{
public:
	ListNode(int value,ListNode next);
	ListNode();
	~ListNode();
	int value;
	ListNode* next;
	/* data */
};

ListNode::ListNode(int value,ListNode next){
	this->value = value;
	this->next = &next;
}


int main(int argc, char const *argv[])
{
	ListNode p4 = new ListNode(4,NULL);
	ListNode p3 = new ListNode(3,p4);
	ListNode p2 = new ListNode(2,p3);
	ListNode p1 = new ListNode(1,p2);
	return 0;
}
